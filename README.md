# 3-1TopViewV2

## 실행 화면과 기능 설명

### 1. 로그인
> 로그인 화면에서 로그인하면 유니티가 http 요청으로
> 아이디 비번을 보내 올바른 값이면 스프링 서버의 (유니티에서 보낸 요청임을 확인하기위한) 암호값 하나와
> 유저 관련 정보가 응답된다.


![image](https://github.com/fpsgo7/GameWebPageProject_GamePartSingle/assets/101778043/61612322-4668-491f-8327-f4825535de59)

### 2. 캐릭터 생성
> 만약 처음 로그인하여 게임캐릭터가 없다면 캐릭터 생성창이 뜬다.
> 이름 입력후 생성하면 http 요청으로 이름값과 스프링 서버로부터 받은 암호값이 같이
> 보내지며 암호값이 올바르고 이름값이 문제없으면 캐릭터가 추가된다.


![image](https://github.com/fpsgo7/GameWebPageProject_GamePartSingle/assets/101778043/9e014f1a-811e-4864-bb4e-ea95874df97b)
### 3. 랭크 불러오기
> 로그인이후 자동으로 랭크화면이 나타나며 랭크 화면은 나타나기전
> 자동으로 http 요청을 통해 스프링 서버로부터 랭킹 정보를 응답받는다.
> 이 정보를 토대로 랭크 화면을 보여준다.


![image](https://github.com/fpsgo7/GameWebPageProject_GamePartSingle/assets/101778043/fb9e8cec-1ef6-4e16-9b1f-53b7ed7dcbc3)


### 4. 캐릭터 화면
> 자신이 생성한 캐릭터의 정보가 나타나며 채팅화면과 랭킹 화면을 전환하는 버튼이 있다.


![image](https://github.com/fpsgo7/GameWebPageProject_GamePartSingle/assets/101778043/740079ef-b55c-48df-939d-64c0c4757ce6)



### 5. 채팅
> 채팅 기능은 유니티에서 값을 입력후 http 요청을 통해 입력한 값을 구글의 스프레드 시트에 보내어 스프레드 시트의 스크립트가
> 받아 값을 스프레드 시트에 저장하는 형식으로 채팅을 받으며
> 유니티에서 get 요청으로 구글 스프레드 시트로 부터 채팅값을 받아 채팅화면에 채팅 내용을 보여주는 형식이다.


![image](https://github.com/fpsgo7/GameWebPageProject_GamePartSingle/assets/101778043/e815fcd0-418d-4826-a4df-d6866f67398f)
