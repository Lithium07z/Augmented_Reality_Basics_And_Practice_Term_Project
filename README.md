# Augmented Reality Basics and Practice — Term Project

2023년 2학기 **증강현실 기초 및 실습(Augmented Reality Basics and Practice)** 수업에서 개발한 Unity 기반 모바일 AR 텀 프로젝트입니다.

Unity의 **AR Foundation / ARCore**를 기반으로 이미지 트래킹, 평면 인식, GPS 위치 정보 및 Firebase Realtime Database를 결합하여 현실 공간에서 AR 콘텐츠와 상호작용할 수 있는 기능을 구현했습니다.

## 프로젝트 개요

본 프로젝트에서는 모바일 AR 환경에서 사용할 수 있는 여러 핵심 기능을 구현하고 통합했습니다.

주요 구현 내용은 다음과 같습니다.

* AR Foundation 기반 **평면 인식 및 Raycast**
* Reference Image 기반 **이미지 트래킹**
* 실제 GPS 위치 정보 활용
* Firebase Realtime Database를 이용한 AR 콘텐츠 데이터 관리
* 이미지 마커에 대응하는 3D 오브젝트 생성
* AR 환경에서의 몬스터 탐색 및 전투
* 터치 입력 기반 AR 오브젝트 조작
* AR 공간에 사진을 배치하는 Gallery 기능
* AR 콘텐츠 선택, 이동, 크기 조절 및 삭제

---

## 주요 기능

### 1. AR Image Tracking

`ARTrackedImageManager`를 이용해 등록된 Reference Image를 인식하고, 인식된 이미지의 위치와 자세를 기준으로 AR 오브젝트를 생성합니다.

이미지가 이동하거나 회전하면 배치된 AR 콘텐츠 역시 트래킹 결과를 따라 갱신됩니다.

---

### 2. GPS 기반 콘텐츠 탐색

Android 위치 권한을 요청한 뒤 Unity Location Service를 이용하여 사용자의 현재 위도와 경도를 가져옵니다.

수집된 위치 정보는 Firebase에 저장된 AR 콘텐츠의 위치 데이터와 비교하는 데 사용됩니다.

이를 통해 특정 위치에서만 특정 AR 콘텐츠가 활성화되는 **Location-based AR** 구조를 구현했습니다.

---

### 3. Firebase Realtime Database

Firebase Realtime Database를 이용하여 AR 콘텐츠의 정보를 관리합니다.

각 콘텐츠에는 다음과 같은 정보가 저장됩니다.

* Object Name
* Latitude
* Longitude
* Capture State

사용자의 현재 위치와 데이터베이스에 등록된 위치를 비교하여 조건을 만족하는 콘텐츠를 검색하고, 해당 3D Prefab을 AR 환경에 생성합니다.

획득된 콘텐츠의 상태 역시 Firebase를 통해 갱신할 수 있도록 구현했습니다.

---

### 4. AR Monster Interaction

이미지 마커와 위치 조건을 만족하면 대응하는 몬스터가 AR 공간에 등장합니다.

사용자는 화면 중앙의 Raycast를 이용하여 몬스터를 선택할 수 있으며 다음 정보를 확인할 수 있습니다.

* 몬스터 이름
* 몬스터 설명
* 전투 가능 여부

전투 모드에서는 화면 터치를 통해 카메라 방향으로 Projectile을 발사하며 몬스터와 상호작용합니다.

---

### 5. Plane Detection & AR Object Placement

`ARPlaneManager`와 `ARRaycastManager`를 이용하여 주변의 평면을 탐지합니다.

AR Plane이 발견되면 Scan 상태에서 Main 상태로 전환하며, 검출된 공간을 기반으로 AR 콘텐츠를 배치할 수 있습니다.

---

### 6. AR Gallery

AR 공간에 이미지를 배치하고 편집할 수 있는 Gallery 기능도 구현했습니다.

사용자는 다음과 같은 작업을 수행할 수 있습니다.

* 이미지 선택
* AR Plane 위에 이미지 배치
* 배치된 이미지 선택
* 다른 위치로 이동
* Pinch Gesture를 이용한 크기 조절
* 이미지 교체
* 이미지 삭제

이미지 이동 시 AR Raycast 결과의 Plane Position과 Normal을 이용하여 실제 표면에 맞게 오브젝트의 위치와 방향을 조정합니다.

---

## 동작 구조

```text
Application Start
        │
        ▼
GPS / AR Session Initialization
        │
        ▼
Environment / Image Scan
        │
        ├── Plane Detection
        │
        └── Reference Image Detection
        │
        ▼
GPS Position Acquisition
        │
        ▼
Firebase Data Search
        │
        ▼
AR Content Matching
        │
        ▼
3D Object Instantiation
        │
        ▼
User Interaction
        ├── Information
        ├── Battle
        ├── Move
        ├── Resize
        └── Delete / Replace
```

---

## 개발 환경

| 항목           | 환경                               |
| ------------ | -------------------------------- |
| Engine       | Unity 2022.3.8f1                 |
| Language     | C#                               |
| Rendering    | Universal Render Pipeline (URP)  |
| AR Framework | Unity AR Foundation              |
| Android AR   | ARCore                           |
| Database     | Firebase Realtime Database       |
| Platform     | Android                          |
| Input        | Unity Input System / Touch Input |

### 주요 Unity Packages

* AR Foundation 5.0.7
* ARCore XR Plugin 5.0.7
* XR Plug-in Management 4.4.0
* Universal RP 14.0.8
* TextMeshPro 3.0.6
* Localization 1.4.5

---

## 주요 스크립트

```text
Assets/Scripts/
├── GPS_Manager.cs
├── DB_Manager.cs
├── ImageScanMode.cs
├── MonstersMainMode.cs
├── Monster.cs
├── Player.cs
│
├── ScanMode.cs
├── MainMode.cs
├── InteractionController.cs
├── UIController.cs
│
├── EditPicture.cs
├── MovePicture.cs
├── ReSizePicture.cs
├── ImageButtons.cs
└── ImagesData.cs
```

### AR / Location

**`GPS_Manager.cs`**
Android 위치 권한과 Unity Location Service를 관리하고 현재 GPS 좌표를 갱신합니다.

**`DB_Manager.cs`**
Firebase Realtime Database와 통신하고 위치 기반 AR 콘텐츠 데이터를 저장 및 검색합니다.

**`ImageScanMode.cs`**
AR Reference Image Tracking을 관리합니다.

**`MonstersMainMode.cs`**
Tracked Image, GPS 및 Firebase 데이터를 이용하여 몬스터를 생성하고 메인 AR 상호작용을 관리합니다.

### Interaction

**`Player.cs`**
AR 전투에서 플레이어의 체력, 공격 및 Projectile 발사를 담당합니다.

**`ScanMode.cs`**
AR Plane이 발견될 때까지 공간 스캔 상태를 유지합니다.

**`InteractionController.cs`**
Scan, Main, Edit 등의 애플리케이션 Interaction Mode를 전환합니다.

### AR Gallery

**`MovePicture.cs`**
AR Raycast를 이용하여 배치된 사진을 다른 Plane 위치로 이동합니다.

**`ReSizePicture.cs`**
두 손가락 Pinch Gesture를 이용한 이미지 크기 조절 기능을 제공합니다.

**`EditPicture.cs`**
배치된 사진 선택, 교체 및 삭제 기능을 관리합니다.

---

## 실행 환경 및 주의사항

본 프로젝트는 수업 텀 프로젝트 당시의 개발 환경을 보존한 저장소입니다.

Firebase Database URL과 같은 일부 서비스 설정 정보는 공개 저장소에서 제거되어 있으므로, Firebase 기능을 다시 실행하려면 별도의 Firebase 프로젝트 설정이 필요합니다.

또한 프로젝트에서 사용했던 일부 외부 Unity Package는 기존 개발 PC의 로컬 경로를 참조하고 있을 수 있으므로, 다른 환경에서 프로젝트를 복원할 경우 해당 패키지를 다시 설치하거나 Package Manager 설정을 수정해야 할 수 있습니다.

권장 Unity 버전:

```text
Unity 2022.3.8f1
```

---

## Project Result

2023년 2학기 텀 프로젝트 결과 발표 자료:

[Term Project Results](https://www.canva.com/design/DAFyzJSPC8o/p6ed3PG58gyu0jMUPdKmSA/view?utm_content=DAFyzJSPC8o&utm_campaign=designshare&utm_medium=link&utm_source=editor)

---

## Repository Purpose

이 저장소는 **증강현실 기초 및 실습 수업에서 AR Foundation을 이용해 구현한 모바일 AR 텀 프로젝트와 실습 결과를 보존하기 위한 저장소**입니다.

AR Tracking, Location-based AR, Firebase 연동 및 모바일 AR Interaction을 하나의 프로젝트에서 실험하고 구현하는 것을 목표로 했습니다.

## 로컬 설정과 서명키

Unity의 Library, Logs 등 생성 파일과 개인 서명키는 Git에 커밋하지 않습니다.
프로젝트를 처음 열면 Unity가 필요한 캐시를 다시 생성합니다.

Android 빌드에 사용할 서명키는 저장소 외부의 안전한 위치에 보관하고 Unity의
Publishing Settings에서 지정하세요. 과거에 공개된 키를 새 배포에 재사용하지 마세요.
이미 출시한 앱에 사용한 키라면 앱 서명키와 업로드 키를 구분하여 배포 플랫폼의
키 교체 절차를 먼저 확인해야 합니다. Git에서 지우는 것만으로 기존 키가 폐기되지는 않습니다.


## Firebase 설정

Firebase Console에서 본인 프로젝트의 Android 설정 파일을 내려받아
`Assets/google-services.json`에 배치하세요. `Assets/StreamingAssets/google-services.json`은
Firebase Unity SDK가 생성하거나 프로젝트의 기존 빌드 절차에 따라 준비합니다.
두 파일과 관련 meta 파일, Firebase가 생성하는
`Assets/Firebase/Editor/res/values/googleservices.xml`은 커밋하지 않습니다.

Google Cloud에서 API 키를 필요한 Firebase API로 제한하고, 사용 중인 Firebase 제품의
Security Rules 및 App Check를 확인하세요. 클라이언트 API 키를 숨기는 것만으로 데이터가
보호되지는 않습니다. Firebase용 공개 키에 Gemini 등 별도 유료 API 권한을 추가하지 마세요.

