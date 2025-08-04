# 🐠 'Under-the-Sea' Project

**Under-the-Sea**는 Unity로 제작되었습니다. 
'쿠키런'과 유사한 방식으로, 슬라임이 다양한 장애물을 피하며 바다로 돌아가는 액션 러닝 게임입니다.

---

<img width="1580" height="882" alt="image" src="https://github.com/user-attachments/assets/bf62bc6a-1fac-4f41-93a8-e537039da026" />

--- 
## 📑 목차

1. [게임 소개](#게임-소개)
2. [플레이 방법](#플레이-방법)
3. [주요 기능](#주요-기능)
4. [개발 환경](#개발-환경)
5. [프로젝트 구조 (Git Flow)](#프로젝트-구조-git-flow)
6. [팀원 및 역할 분담](#팀원-및-역할-분담)
7. [스크린샷](#스크린샷)
8. [추가 참고사항](#추가-참고사항)

---

## 🎮 게임 소개

- **장르**: 2D Side-scrolling Action Runner (횡스크롤 액션 러너)
- **플랫폼**: PC (Unity Standalone)
- **컨셉**: 깊고 어두운 동굴을 지나, 늪지대에 온갖 장애물을 피한 뒤 해변으로 도착해 그토록 바라던 바다로 돌아가는 슬라임입니다.
- **목표**: 스테이지별 난관을 넘어 최대한 멀리 전진하는 것이 목표입니다.

---

## 🕹️ 플레이 방법

- **스페이스바**: 점프
- **쉬프트키** : 슬라이드
- **장애물 회피 및 물방울 아이템 획득으로 인한 점수 추가**
- 일정 거리마다 스테이지 전환 (Stage 1 → 2 → 3)

---

## 🚀 주요 기능

- 배경 루핑 시스템 (BgLooper)
- 다양한 장애물 및 랜덤 생성 로직
- 스테이지 별 배경 전환
- 게임오버 및 재시작 기능
- 기본 UI (메인화면, 스테이지 전환, 게임오버 팝업 등)

---

## 🛠 개발 환경

- **엔진**: Unity **2022.3.17f1**
- **언어**: C#
- **버전 관리**: Git / GitHub

---

## 🗂 프로젝트 구조 (Git Flow)

```text
main
└── develop
    ├── JJW_UI
    ├── jinwoo
    ├── jongmin3
    ├── lwh
    └── jongmin1111111111

```
---
## 👨‍👩‍👧‍👦 팀원 및 역할 분담

| 이름 (브랜치)                         | 역할 설명 |
|-------------------------------------|-----------|
| **전진우** (`jinwoo`)                 | - 장애물 랜덤 생성 로직 구현<br>- ex)Top/Bottom_Obstacle 분리 및 제약 조건 적용<br>- ex)스테이지 전환 및 BgLooper 구성 |
| **한종민** (`jongmin3`, `jongmin1111111111`) | - ex)플레이어 이동, 점프, 충돌 판정 구현<br>- ex)애니메이션 및 상태 관리<br>- ex)사망 처리 및 게임오버 시스템 |
| **이원혁** (`lwh`)                    | - ex)배경 아트 디자인 및 타일맵 구성<br>- ex)스테이지별 배경 전환 시스템 |
| **정지원** (`JJW_UI`)                 | - ex)메인화면, 게임오버, 스테이지 UI 설계<br>- ex)버튼 이벤트 처리 및 UI 기능 연결 |
| **전체 팀**                         | - ex)게임 기획, 버그 수정, 테스트 및 피드백 공유 등 협업 전반 |

> 🧩 역할은 개발 중 상호 협의에 따라 유동적으로 일부 교차되었을 수 있습니다.

---

## 🖼️ 스크린샷

| 메인화면 | 인게임 화면 |
|----------|-------------|
| <img width="1580" height="882" alt="image" src="https://github.com/user-attachments/assets/bf62bc6a-1fac-4f41-93a8-e537039da026" /> |<img width="1574" height="880" alt="image" src="https://github.com/user-attachments/assets/2c19d390-07ff-4206-840e-7d9d51aa1aa2" /> |

| 스테이지 1 | 스테이지 2 | 스테이지 3 | 
|------------|------------|------------|
| <img width="1580" height="881" alt="image" src="https://github.com/user-attachments/assets/f1030f8b-3ca0-415e-9be6-921764ad083f" /> | <img width="1582" height="881" alt="image" src="https://github.com/user-attachments/assets/afd054c7-1ec6-4108-982e-81d3a020fcfe" />| <img width="1580" height="884" alt="image" src="https://github.com/user-attachments/assets/154683c4-e9e1-4b87-a6c3-74c6fb468dba" />

| 게임오버 화면 |
|--------------|
|<img width="1582" height="883" alt="image" src="https://github.com/user-attachments/assets/69251242-12bf-4225-b03c-e8b5937a50ba" />|
> 📁 `screenshots/` 폴더에 위 이미지들이 들어 있어야 정상적으로 표시됩니다.

---

## 📌 추가 참고사항
