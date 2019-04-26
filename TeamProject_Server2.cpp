﻿#include "pch.h"
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
	em_USER_CAMERA,
	em_USER_FKEY,
	em_USER_ZKEY,
	em_USER_XKEY,
	em_USER_CKEY,
	em_USER_VKEY,
	em_USER_RKEY,
	em_USER_GAMEDEATH,
	em_USER_RESOURCE,
	em_USER_STATUS,
	em_OBJECT_TREE,
	em_OBJECT_STONE,
	em_OBJECT_BULLET,
};
struct sFlag {
	char flag;
	sFlag(char f = 0) :flag(f) {};
};
struct sLogin :sFlag {
	int idx;
	SOCKET sock;
	int status;
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
int a;
list<sLogin> loginlist;
list<sMatching> matchinglist;
HANDLE hMutex;
list<sMatching>::iterator miter;
void vMatching(SOCKET sock);
void vTeam(SOCKET sock);
void vMessage(SOCKET sock, char* buffer);
void vLogin(SOCKET sock, char* buffer);

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

	hComPort = CreateIoCompletionPort(INVALID_HANDLE_VALUE, NULL, 0, 0);//이밴트 큐 형태로 만드는것.(LPPER_IO_DATA)malloc(sizeof(PER_IO_DATA));
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

		ioInfo = new PER_IO_DATA;
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
			if (bytesTrans <= 0)//EOF 전송 시
			{
				//로그인 관련
				list<sLogin>::iterator iterlogin = loginlist.begin();
				WaitForSingleObject(hMutex, INFINITE);
				while (iterlogin != loginlist.end()) {
					if (iterlogin->sock == sock) {
						loginlist.erase(iterlogin);
						break;
					}
					iterlogin++;
				}
				ReleaseMutex(hMutex);
				//매치관련
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
				delete(handleInfo);
				delete(ioInfo);
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
				vLogin(sock, ioInfo->buffer);
			
			}
			else if (flag.flag == em_USER_MATCHING) {
				vMatching(sock);
				
			}
			else if (flag.flag == em_USER_TEAM) {
				vTeam(sock);
				
			}
			else if (flag.flag == em_USER_POSITION) {//위치 정보 전달
				vMessage(sock, ioInfo->buffer);
			}
			else if (flag.flag == em_USER_MESSAGE) {

			}
			else if (flag.flag == em_USER_RAYSHOT) {
				vMessage(sock, ioInfo->buffer);
				
			}
			else if (flag.flag == em_USER_CAMERA) {
				vMessage(sock, ioInfo->buffer);
				
			}//카메라 각도 전송
			else if (flag.flag == em_USER_FKEY) {
				vMessage(sock, ioInfo->buffer);
			}//Fkey에 따른 정보 전송
			else if (flag.flag == em_USER_ZKEY) {
				vMessage(sock, ioInfo->buffer);
				
			}
			else if (flag.flag == em_USER_XKEY) {
				vMessage(sock, ioInfo->buffer);
				
				//Xkey에 따른 정보 전송
			}
			else if (flag.flag == em_USER_CKEY) {
				vMessage(sock, ioInfo->buffer);
				
			}
			else if (flag.flag == em_USER_VKEY) {
				vMessage(sock, ioInfo->buffer);
				
			}
			else if (flag.flag == em_USER_RKEY) {
				vMessage(sock, ioInfo->buffer);
				
			}
			else if (flag.flag == em_USER_GAMEDEATH) {
				vMessage(sock, ioInfo->buffer);
				
			}
			else if (flag.flag == em_USER_RESOURCE) {
				vMessage(sock, ioInfo->buffer);
				
			}
			else if (flag.flag == em_USER_STATUS) {
				vMessage(sock, ioInfo->buffer);
				
			}
			else if (flag.flag == em_OBJECT_TREE) {
				vMessage(sock, ioInfo->buffer);
				
			}
			else if (flag.flag == em_OBJECT_STONE) {
				vMessage(sock, ioInfo->buffer);
				
			}
			else if (flag.flag == em_OBJECT_BULLET) {
				vMessage(sock, ioInfo->buffer);
				
			}
			//WSASend(sock, &(ioInfo->wsabuf), 1, NULL, 0, &(ioInfo->overlapped), NULL);
			
			//delete(ioInfo);
			//데이터 받기
			//ioInfo = new PER_IO_DATA;
			memset(&(ioInfo->overlapped), 0, sizeof(OVERLAPPED));
			ioInfo->wsabuf.len = BUF_SIZE;
			ioInfo->wsabuf.buf = ioInfo->buffer;
			ioInfo->rwMode = READ;
			WSARecv(sock, &(ioInfo->wsabuf), 1, NULL, &flags, &(ioInfo->overlapped), NULL);
			
		}
		else {
			puts("message sent!");
			//delete(ioInfo);
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
	LPPER_IO_DATA ioInfo = new PER_IO_DATA;
	memset(&(ioInfo->overlapped), 0, sizeof(OVERLAPPED));
	ioInfo->wsabuf.len = BUF_SIZE;
	ioInfo->wsabuf.buf = ioInfo->buffer;
	ioInfo->rwMode = WRITE;
	memcpy(ioInfo->buffer, buffer, sizeof(ioInfo->buffer));
	WSASend(UserSocket, &(ioInfo->wsabuf), 1, NULL, 0, &(ioInfo->overlapped), NULL);
	delete(ioInfo);
}
void vMatching(SOCKET sock) {
	sMatching match;
	for (int a = 0; a < 4; a++) {
		match.UserSocket[a] = 0;
	}

	if (matchinglist.size() == 0) {
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
				c_send(iter->UserSocket[1], (char*)&match);
				for (int a = 0; a < 2; a++)
					c_send(iter->UserSocket[a], (char*)&match);
				break;
			}
			else if (iter->UserSocket[2] == 0) {
				iter->UserSocket[2] = sock;
				for (int b = 0; b < 2; b++)
					c_send(iter->UserSocket[2], (char*)&match);
				for (int a = 0; a < 3; a++)
					c_send(iter->UserSocket[a], (char*)&match);
				break;
			}
			else if (iter->UserSocket[3] == 0) {
				iter->UserSocket[3] = sock;
				for (int b = 0; b < 3; b++)
					c_send(iter->UserSocket[3], (char*)&match);
				for (int a = 0; a < 4; a++)
					c_send(iter->UserSocket[a], (char*)&match);
				break;
			}
			iter++;
			if (iter == matchinglist.end()) {
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
void vLogin(SOCKET sock, char* buffer) {
	sLogin login;
	memcpy(&login, buffer, sizeof(sLogin));
	if (loginlist.size() == 0) {
		login.status = 1;
		login.sock = sock;
		loginlist.push_back(login);
		c_send(sock, (char*)&login);
	}
	else {
		list<sLogin>::iterator iter = loginlist.begin();
		while (iter != loginlist.end()) {
			if (iter->idx == login.idx) {
				login.status = 2;
				c_send(sock, (char*)&login);
				break;
			}
			iter++;
			if (iter == loginlist.end()) {
				login.status = 1;
				login.sock = sock;
				loginlist.push_back(login);
				c_send(sock, (char*)&login);
				break;
			}
		}
	}
}
void vMessage(SOCKET sock, char* buffer) {
	list<sMatching>::iterator miter = matchinglist.begin();
	while (miter != matchinglist.end()) {
		if (miter->UserSocket[0] == sock || miter->UserSocket[1] == sock || miter->UserSocket[2] == sock || miter->UserSocket[3] == sock) {

			for (a = 0; a < 4; a++) {
				if (miter->UserSocket[a] == sock)
					continue;
				else {
					c_send(miter->UserSocket[a], buffer);
				}
			}
			break;
		}
		miter++;
	}
}