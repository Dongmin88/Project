
#include "pch.h"
#include <iostream>
#include <stdio.h>
#include <stdlib.h>
#include <list>
#include <process.h>
#include <winsock2.h>
#include <windows.h>
#include <string.h>

using namespace std;
#define BUF_SIZE 256
#define READ 3
#define WRITE 5
enum eMSG {
	em_USER_LOGIN = 1,
	em_USER_MATCHING,
	em_USER_TEAM,
	em_USER_POSITION,
	em_USER_MESSAGE,
	em_USER_RAYSHOT,
};
struct sFlag {
	char flag;
	sFlag(char f = 0) :flag(f) {};
};
struct sLogin :sFlag {
	SOCKET sock;
	sLogin() :sFlag(em_USER_LOGIN) {};
};
struct sUser {
	SOCKET sock;
};
struct sPosition :sFlag {
	int player;
	float px;
	float py;
	float pz;
	float dx;
	float dy;
	float dz;
	sPosition() : sFlag(em_USER_POSITION) {};
};
struct sTeam :sFlag {
	int team;
	sTeam() : sFlag(em_USER_TEAM) {};
};
struct sMatching :sFlag {
	SOCKET UserSocket[4];
	sMatching() :sFlag(em_USER_MATCHING) {};
};
struct sMessage :sFlag {
	char msg[BUF_SIZE];
	sMessage() :sFlag(em_USER_MESSAGE) {};
};
struct sRayshot : sFlag
{
	int player;
	sRayshot() :sFlag(em_USER_RAYSHOT) {};
};
typedef struct {//socket info
	SOCKET hClntSock;
	SOCKADDR_IN clntAdr;
}PER_HANDLE_DATA, *LPPER_HANDLE_DATA;

typedef struct {//buffer info
	OVERLAPPED overlapped;//이밴트 방식
	WSABUF wsabuf;//IOCP에서 이용 int len과 char* buf가 있음
	char buffer[BUF_SIZE];
	int rwMode;//READ or WRITE
}PER_IO_DATA, *LPPER_IO_DATA;//LP=포인터라고 생각

unsigned WINAPI EchoThreadMain(LPVOID CompletionPortIO);
void ErrorHandling(const char *message);

list<sUser> listuser;
list<sMatching> matchinglist;
HANDLE hMutex;
void vMatching(SOCKET sock);
void vTeam(SOCKET sock);
void vPosition(SOCKET sock, char* buffer);

int main()
{
	WSADATA wsaData;
	HANDLE hComPort;
	SYSTEM_INFO sysInfo;
	LPPER_IO_DATA ioInfo;
	LPPER_HANDLE_DATA handleInfo;

	SOCKET hServSock;
	SOCKADDR_IN servAdr;
	hMutex = CreateMutex(NULL, FALSE, NULL);
	unsigned long recvBytes, i, flags = 0;
	if (WSAStartup(MAKEWORD(2, 2), &wsaData) != 0)
		ErrorHandling("WSAStartup() error!");

	hComPort = CreateIoCompletionPort(INVALID_HANDLE_VALUE, NULL, 0, 0);//이밴트 큐 형태로 만드는것.
	GetSystemInfo(&sysInfo);//컴퓨터 기본 정보 받기
	for (i = 0; i < sysInfo.dwNumberOfProcessors; i++)//코어 갯수 만큼 쓰레드 생성.
		_beginthreadex(NULL, 0, EchoThreadMain, (LPVOID)hComPort, 0, NULL);

	hServSock = WSASocket(AF_INET, SOCK_STREAM, 0, NULL, 0, WSA_FLAG_OVERLAPPED);
	memset(&servAdr, 0, sizeof(servAdr));
	servAdr.sin_family = AF_INET;
	servAdr.sin_addr.s_addr = htonl(INADDR_ANY);
	servAdr.sin_port = htons(10000);

	bind(hServSock, (SOCKADDR*)&servAdr, sizeof(servAdr));
	listen(hServSock, 5);

	while (1) {
		SOCKET hClntSock;
		SOCKADDR_IN clntAdr;
		int addrLen = sizeof(clntAdr);

		hClntSock = accept(hServSock, (SOCKADDR*)&clntAdr, &addrLen);
		handleInfo = (LPPER_HANDLE_DATA)malloc(sizeof(PER_HANDLE_DATA));
		handleInfo->hClntSock = hClntSock;
		memcpy(&(handleInfo->clntAdr), &clntAdr, addrLen);

		CreateIoCompletionPort((HANDLE)hClntSock, hComPort, (DWORD)handleInfo, 0);

		ioInfo = (LPPER_IO_DATA)malloc(sizeof(PER_IO_DATA));
		memset(&(ioInfo->overlapped), 0, sizeof(OVERLAPPED));
		ioInfo->wsabuf.len = BUF_SIZE;
		ioInfo->wsabuf.buf = ioInfo->buffer;
		ioInfo->rwMode = READ;
		WSARecv(handleInfo->hClntSock, &(ioInfo->wsabuf), 1, &recvBytes, &flags, &(ioInfo->overlapped), NULL);
	}
	return 0;
}

unsigned WINAPI EchoThreadMain(LPVOID pComPort) {
	HANDLE hComPort = (HANDLE)pComPort;
	SOCKET sock;
	DWORD bytesTrans;
	LPPER_HANDLE_DATA handleInfo;
	LPPER_IO_DATA ioInfo;
	DWORD flags = 0;

	while (1)
	{
		GetQueuedCompletionStatus(hComPort, &bytesTrans, (LPDWORD)&handleInfo, (LPOVERLAPPED*)&ioInfo, INFINITE);//이밴트 큐 주소값을 정수로 등록
		sock = handleInfo->hClntSock;

		if (ioInfo->rwMode == READ) {
			puts("message received!");
			if (bytesTrans == 0)//EOF 전송 시
			{
				list<sMatching>::iterator iter = matchinglist.begin();
				WaitForSingleObject(hMutex, INFINITE);
				while (iter != matchinglist.end()) {
					if (iter->UserSocket[0] == sock) {
						matchinglist.erase(iter);
						break;
					}
					else if (iter->UserSocket[1] == sock) {
						iter->UserSocket[1] = 0;
					}
					else if (iter->UserSocket[2] == sock) {
						iter->UserSocket[2] = 0;
					}
					else if (iter->UserSocket[3] == sock) {
						iter->UserSocket[3] = 0;
					}
					iter++;
				}
				ReleaseMutex(hMutex);

				closesocket(sock);
				free(handleInfo);
				free(ioInfo);
				continue;
			}
			memset(&(ioInfo->overlapped), 0, sizeof(OVERLAPPED));
			ioInfo->wsabuf.len = bytesTrans;
			ioInfo->rwMode = WRITE;
			//Flag check
			sFlag flag;
			memcpy(&flag.flag, ioInfo->buffer, sizeof(flag.flag));
			if (flag.flag == em_USER_LOGIN) {
				//유저 리스트 추가
				sUser user;
				user.sock = sock;
				listuser.push_back(user);
				WSASend(sock, &(ioInfo->wsabuf), 1, NULL, 0, &(ioInfo->overlapped), NULL);//버퍼 보내기
			
			}
			else if (flag.flag == em_USER_MATCHING) {
				vMatching(sock);
			}
			else if (flag.flag == em_USER_TEAM) {
				vTeam(sock);
			}
			else if (flag.flag == em_USER_POSITION) {//위치 정보 전달
				vPosition(sock, ioInfo->buffer);
			}
			else if (flag.flag == em_USER_MESSAGE) {
				list<sMatching>::iterator iter = matchinglist.begin();
				while (iter != matchinglist.end()) {
					if (iter->UserSocket[0] == sock || iter->UserSocket[1] == sock || iter->UserSocket[2] == sock || iter->UserSocket[3] == sock) {//채팅창
						sMessage sMsg;
						memcpy(&sMsg, &ioInfo->buffer, sizeof(sMsg));
						break;
					}
					iter++;
				}
			}
			else if (flag.flag == em_USER_RAYSHOT) {

			}
			//WSASend(sock, &(ioInfo->wsabuf), 1, NULL, 0, &(ioInfo->overlapped), NULL);

			//데이터 받기
			ioInfo = (LPPER_IO_DATA)malloc(sizeof(PER_IO_DATA));
			memset(&(ioInfo->overlapped), 0, sizeof(OVERLAPPED));
			ioInfo->wsabuf.len = BUF_SIZE;
			ioInfo->wsabuf.buf = ioInfo->buffer;
			ioInfo->rwMode = READ;
			WSARecv(sock, &(ioInfo->wsabuf), 1, NULL, &flags, &(ioInfo->overlapped), NULL);
		}
		else {
			puts("message sent!");
			free(ioInfo);
		}
	}
	return 0;
}

void ErrorHandling(const char *message) {
	fputs(message, stderr);
	fputc('\n', stderr);
	exit(1);
}

void c_send(SOCKET UserSocket, char* buffer) {
	LPPER_IO_DATA ioInfo = (LPPER_IO_DATA)malloc(sizeof(PER_IO_DATA));
	memset(&(ioInfo->overlapped), 0, sizeof(OVERLAPPED));
	ioInfo->wsabuf.len = BUF_SIZE;
	ioInfo->wsabuf.buf = ioInfo->buffer;
	ioInfo->rwMode = WRITE;
	memcpy(ioInfo->buffer, buffer, sizeof(ioInfo->buffer));
	WSASend(UserSocket, &(ioInfo->wsabuf), 1, NULL, 0, &(ioInfo->overlapped), NULL);
}
void vMatching(SOCKET sock) {
	sMatching match;
	for (int a = 0; a < 4; a++) {
		match.UserSocket[a] = 0;
	}

	if (matchinglist.size() == 0) {
		memset(&match, 0, sizeof(match));
		match.UserSocket[0] = sock;
		matchinglist.push_back(match);
		c_send(match.UserSocket[0], (char*)&match);
	}
	else {
		/*WaitForSingleObject(hMutex, INFINITE);*/
		list<sMatching>::iterator iter = matchinglist.begin();
		while (iter != matchinglist.end()) {
			if (iter->UserSocket[1] == 0) {
				iter->UserSocket[1] = sock;
				for (int a = 0; a < 2; a++)
					c_send(iter->UserSocket[a], (char*)&match);
				break;
			}
			else if (iter->UserSocket[2] == 0) {
				iter->UserSocket[2] = sock;
				for (int a = 0; a < 3; a++)
					c_send(iter->UserSocket[a], (char*)&match);
				break;
			}
			else if (iter->UserSocket[3] == 0) {
				iter->UserSocket[3] = sock;
				for (int a = 0; a < 4; a++)
					c_send(iter->UserSocket[a], (char*)&match);
				break;
			}
			iter++;
			if (iter == matchinglist.end()) {
				sMatching match;
				memset(&match, 0, sizeof(match));
				match.UserSocket[0] = sock;
				matchinglist.push_back(match);
				break;
			}
		}
	}

}
void vTeam(SOCKET sock) {
	list<sMatching>::iterator iter = matchinglist.begin();
	while (iter != matchinglist.end()) {
		if (iter->UserSocket[0] == sock || iter->UserSocket[1] == sock || iter->UserSocket[2] == sock || iter->UserSocket[3] == sock) {//
			sTeam pos;
			for (int a = 0; a < 4; a++) {
				pos.team = a + 1;//팀 번호가 1부터
				c_send(iter->UserSocket[a], (char*)&pos);
			}
			break;
		}
		iter++;
	}
}
void vPosition(SOCKET sock,char* buffer) {
	list<sMatching>::iterator iter = matchinglist.begin();
	while (iter != matchinglist.end()) {
		if (iter->UserSocket[0] == sock || iter->UserSocket[1] == sock || iter->UserSocket[2] == sock || iter->UserSocket[3] == sock) {
			for (int a = 0; a < 4; a++) {
				if (iter->UserSocket[a] == sock)
					continue;
				else {
					c_send(iter->UserSocket[a], buffer);
				}
			}
		}
		iter++;
	}
}
void vRayshot(SOCKET sock, char* buffer) {
	list<sMatching>::iterator iter = matchinglist.begin();
	while (iter != matchinglist.end()) {
		if (iter->UserSocket[0] == sock || iter->UserSocket[1] == sock || iter->UserSocket[2] == sock || iter->UserSocket[3] == sock) {
			for (int a = 0; a < 4; a++) {
				if (iter->UserSocket[a] == sock)
					continue;
				else {
					c_send(iter->UserSocket[a], buffer);
				}
			}
		}
		iter++;
	}
}
