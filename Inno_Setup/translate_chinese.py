# -*- coding: utf-8 -*-
"""
Korean.XML 내 중국어 항목 및 미번역 영어 UI 항목을 한국어로 교체
"""

file = r'D:\Documents\Smart_LM_China\Inno_Setup\Korean.XML'

with open(file, encoding='utf-8') as f:
    content = f.read()

t = [
    # ── 중국어 항목 번역 ──────────────────────────────────────────────
    # XML 주석 내 중국어는 건드리지 않고 태그 내 값만 교체

    # 서명 관련
    ('RecordSignature>?名<', 'RecordSignature>서명<'),
    ('RecordSignature_0>未?名<', 'RecordSignature_0>미서명<'),
    ('RecordSignature_1>已?名<', 'RecordSignature_1>서명됨<'),
    ('RecordSignature_2>已??<', 'RecordSignature_2>검증됨<'),
    ('RecordSignature_3>????<', 'RecordSignature_3>검증 오류<'),

    # 출입 기록 보고서
    ('PrintRecord>打印出入???表<', 'PrintRecord>출입 기록 보고서 인쇄<'),
    ('OutputRecord>?出出入???表<', 'OutputRecord>출입 기록 보고서 내보내기<'),
    ('OutputTotalRecord>?出出入?????表<', 'OutputTotalRecord>출입 기록 통계 보고서 내보내기<'),
    ('PrintFaceRecord>打印出入??照片?表<', 'PrintFaceRecord>출입 기록 사진 보고서 인쇄<'),
    ('OutputFaceRecord>?出出入??照片?表<', 'OutputFaceRecord>출입 기록 사진 보고서 내보내기<'),
    ('SearchRecordLog>??出入???表<', 'SearchRecordLog>출입 기록 보고서 조회<'),

    # 이벤트 코드
    ('lblEventCode>事件：<', 'lblEventCode>이벤트:<'),
    ('cmbEventCodeToolTip>支持多?件?入，使用逗?分隔，示例： 刷???,人???<',
     'cmbEventCodeToolTip>여러 조건 입력 지원, 쉼표로 구분. 예: 카드 인증, 얼굴 인증<'),

    # 권한 그룹 로그
    ('UpdateEmplAccessGroupLog>修改人??限?: %PName (%PCode)； 新?限?:%Group.<',
     'UpdateEmplAccessGroupLog>사용자 출입 그룹 변경: %PName (%PCode); 새 그룹: %Group.<'),
    ('DeleteEmplAccessGroupLog>?除人??限?: %PName (%PCode)；所??限?:%Group.<',
     'DeleteEmplAccessGroupLog>사용자 출입 그룹 삭제: %PName (%PCode); 소속 그룹: %Group.<'),

    # 페이지 번호 (제1 → 1페이지)
    ('SaveTemplate3_20190112_1>第<', 'SaveTemplate3_20190112_1>페이지<'),
    ('SaveTemplate2_20190112_1>第<', 'SaveTemplate2_20190112_1>페이지<'),
    ('SaveTemplate1_20190112_1>第<', 'SaveTemplate1_20190112_1>페이지<'),
    ('SaveTemplate0_20190112_1>第<', 'SaveTemplate0_20190112_1>페이지<'),
    ('SaveTemplateTmp_20190112_1>第<', 'SaveTemplateTmp_20190112_1>페이지<'),

    # 오류 메시지 내 중국어
    ('CheckInput_7>未The device to be operated is not selected!<',
     'CheckInput_7>조작할 장치가 선택되지 않았습니다!<'),

    # ── Image Clip 창 (FrmEmplListInfo_ImageCut) 한글화 ─────────────
    ('FormCaption>Image Clip<', 'FormCaption>이미지 자르기<'),
    ('ZoomIn>Zoom In<', 'ZoomIn>확대<'),
    ('ZoomOut>Zoom Out<', 'ZoomOut>축소<'),
    ('SelectImage>ReSelect<', 'SelectImage>다시 선택<'),
    ('SaveImage>Save<', 'SaveImage>저장<'),
    ('Quality>Quality:<', 'Quality>화질:<'),
    ('Rotate>Rotate<', 'Rotate>회전<'),
    ('ImageErr1>Poor photo, please change a new one!<',
     'ImageErr1>사진 품질이 낮습니다. 다른 사진을 선택하세요!<'),
    ('ImageErr2>The quality of this photo is average, please change a new one!<',
     'ImageErr2>사진 품질이 보통입니다. 다른 사진을 선택하세요!<'),
    ('ImageErr3>If the height or width of the photo is too small, the software will automatically fill the white edge.<',
     'ImageErr3>사진의 가로 또는 세로가 너무 작으면 자동으로 흰색 여백이 채워집니다.<'),
    ('ImageErr4>The photo is too blurry, please change a new one!<',
     'ImageErr4>사진이 너무 흐립니다. 다른 사진을 선택하세요!<'),
    ('FaceCheckErr1>No face detected!<', 'FaceCheckErr1>얼굴이 감지되지 않았습니다!<'),
    ('FaceCheckErr2>The face of this photo is too far away, please change a new one<',
     'FaceCheckErr2>얼굴이 너무 멀리 있습니다. 다른 사진을 선택하세요<'),
    ('FaceCheckErr3>Multiple face regions detected!<',
     'FaceCheckErr3>여러 얼굴이 감지되었습니다!<'),
    ('FaceCheckOver>Photo detection passed<', 'FaceCheckOver>사진 검사 통과<'),

    # ── Photo Tip 창 (FrmEmplListInfo_ImageMinTip) 한글화 ────────────
    ('FormCaption>Photo Tip<', 'FormCaption>사진 알림<'),
    ('cmdSave>Use<', 'cmdSave>사용<'),
    ('chkNotShow>No more tips<', 'chkNotShow>다시 표시 안 함<'),
    ('lblTipTitle>Warm Prompt<', 'lblTipTitle>안내<'),

    # ── 사용자 추가 폼 미번역 항목 ────────────────────────────────────
    ('LblEmplPicSize_Caption>480 X 640 pixel<', 'LblEmplPicSize_Caption>480 X 640 픽셀<'),
    ('LblEmplPic_Caption>Picture:<', 'LblEmplPic_Caption>사진:<'),
    ('SetCaption_1>Save &amp;&amp; Add<', 'SetCaption_1>저장 &amp;&amp; 추가<'),
    ('SetCaption_2>Basic Information<', 'SetCaption_2>기본 정보<'),
    ('SetCaption_3>Name:<', 'SetCaption_3>이름:<'),
    ('SetCaption_4>Card ID:<', 'SetCaption_4>카드 ID:<'),
    ('SetCaption_5>Select<', 'SetCaption_5>선택<'),
    ('SetCaption_6>Other Information<', 'SetCaption_6>기타 정보<'),
    ('SetCaption_7>Password:<', 'SetCaption_7>비밀번호:<'),
    ('SetCaption_8>No.:<', 'SetCaption_8>번호:<'),
    ('SetCaption_9>Card Code:<', 'SetCaption_9>카드 코드:<'),
    ('SetCaption_10>Religion:<', 'SetCaption_10>종교:<'),
    ('SetCaption_11>Gender:<', 'SetCaption_11>성별:<'),
    ('SetCaption_13>Select<', 'SetCaption_13>선택<'),
    ('SetCaption_15>Select<', 'SetCaption_15>선택<'),
    ('SetCaption_16>Delete<', 'SetCaption_16>삭제<'),
    ('SetCaption_17>Spare Card ID:<', 'SetCaption_17>예비 카드 ID:<'),
    ('SetCaption_18>Select<', 'SetCaption_18>선택<'),
    ('SetCaption_19>Spare Card Code:<', 'SetCaption_19>예비 카드 코드:<'),
    ('SetCaption_20>Birthday:<', 'SetCaption_20>생년월일:<'),
    ('SetCaption_21>Country:<', 'SetCaption_21>국가:<'),
    ('SetCaption_22>Birthplace:<', 'SetCaption_22>출생지:<'),
    ('SetCaption_23>ID Type:<', 'SetCaption_23>신분증 종류:<'),
    ('SetCaption_24>ID No.:<', 'SetCaption_24>신분증 번호:<'),
    ('SetCaption_25>Education:<', 'SetCaption_25>학력:<'),
    ('SetCaption_26>Diploma:<', 'SetCaption_26>학위:<'),
    ('SetCaption_27>Graduated From:<', 'SetCaption_27>졸업학교:<'),
    ('SetCaption_28>Hiredate:<', 'SetCaption_28>입사일:<'),
    ('SetCaption_30>Mobile:<', 'SetCaption_30>휴대폰:<'),
    ('SetCaption_31>Email:<', 'SetCaption_31>이메일:<'),
    ('SetCaption_32>Address:<', 'SetCaption_32>주소:<'),
    ('SetCaption_33>Remark:<', 'SetCaption_33>비고:<'),
    ('SetCaption_34>Add %1<', 'SetCaption_34>%1 추가<'),
    ('SetCaption_35>Edit %1<', 'SetCaption_35>%1 편집<'),
    ('SetCaption_36>Save<', 'SetCaption_36>저장<'),
    ('SetCaption_37>Exit<', 'SetCaption_37>종료<'),
    ('SetCaption_38>Basic Profile<', 'SetCaption_38>기본 프로필<'),
    ('SetCaption_39>Advanced Profile<', 'SetCaption_39>상세 프로필<'),
    ('SetCaption_40>Access Level<', 'SetCaption_40>출입 등급<'),
    ('SetCaption_41>Attendance Information<', 'SetCaption_41>근태 정보<'),
    ('SetCaption_43>Select<', 'SetCaption_43>선택<'),
    ('SetCaption_44>Door Name<', 'SetCaption_44>문 이름<'),
    ('SetCaption_45>Tag Name<', 'SetCaption_45>태그명<'),
    ('SetCaption_46>Type<', 'SetCaption_46>유형<'),
    ('SetCaption_47>Expiration<', 'SetCaption_47>만료일<'),
    ('SetCaption_48>Time Zone<', 'SetCaption_48>시간대<'),
    ('SetCaption_49>Times<', 'SetCaption_49>횟수<'),
    ('SetCaption_50>Holiday<', 'SetCaption_50>공휴일<'),
    ('SetCaption_51>Uploaded<', 'SetCaption_51>업로드됨<'),
    ('SetCaption_52>Status<', 'SetCaption_52>상태<'),
    ('SetCaption_53>Add<', 'SetCaption_53>추가<'),
    ('SetCaption_54>Delete<', 'SetCaption_54>삭제<'),
    ('SaveEmpl_3>Give a privilege to the user?<', 'SaveEmpl_3>사용자에게 출입 등급을 부여하시겠습니까?<'),
    ('SaveEmpl_6>Add a new user?<', 'SaveEmpl_6>새 사용자를 추가하시겠습니까?<'),
    ('PassowrdTip>For Card+Password Mode (4 - 8 bits)<',
     'PassowrdTip>카드+비밀번호 모드용 (4~8자리)<'),
    ('IniList_1>Male<', 'IniList_1>남성<'),

    # ── 공통 다이얼로그 Yes/No 버튼 ──────────────────────────────────
    ('SaveEmpl_2>Saved!<', 'SaveEmpl_2>저장되었습니다!<'),
    ('CheckDeptInfo_1>Save<', 'CheckDeptInfo_1>저장<'),
    ('CheckDeptInfo_2>Fail to save the information!<',
     'CheckDeptInfo_2>정보 저장에 실패했습니다!<'),
    ('CheckEmplInput_1>The user name can\'t be empty!<',
     'CheckEmplInput_1>이름을 입력해 주세요!<'),
    ('CheckEmplInput_6>Empty<', 'CheckEmplInput_6>비어 있음<'),
    ('CheckEmplInput_7>The password must be numbers only!<',
     'CheckEmplInput_7>비밀번호는 숫자만 입력 가능합니다!<'),

    # ── 권한 관련 미번역 ──────────────────────────────────────────────
    ('FamDelEmplPwr_Caption>Delete Access Level<', 'FamDelEmplPwr_Caption>출입 등급 삭제<'),
    ('FamAddEmplPwr_Caption>Add Access Level<', 'FamAddEmplPwr_Caption>출입 등급 추가<'),
    ('TableName>Access Level List:<', 'TableName>출입 등급 목록:<'),

    # ── 이미지 파일 필터 ──────────────────────────────────────────────
    ('ImageFileFilter>All Image(*.bmp;*.jpg;)|*.bmp;*.jpg;<',
     'ImageFileFilter>모든 이미지(*.bmp;*.jpg;)|*.bmp;*.jpg;<'),
    ('ImageFileFilter>Viewable image(*.bmp;*.jpg;)|*.bmp;*.jpg;<',
     'ImageFileFilter>이미지 파일(*.bmp;*.jpg;)|*.bmp;*.jpg;<'),

    # ── 사진 오류 메시지 ──────────────────────────────────────────────
    ('PersonnelImageErr>Incorrect Image!\nPersonnel image must contain a clear bust and must not be too close to the camera!\nRecommend to select 2-inch certificate image (Pixel:390*567)!<',
     'PersonnelImageErr>올바르지 않은 이미지입니다!\n사용자 사진은 상반신이 명확해야 하며 카메라에 너무 가까워서는 안 됩니다!\n2인치 증명사진 권장 (픽셀: 390*567)!<'),
    ('PersonnelSmallImageErr>The photo pixel is too low, please change the photo!\nA photo cannot be less than 240*320 pixels.<',
     'PersonnelSmallImageErr>사진 해상도가 너무 낮습니다. 다른 사진을 선택하세요!\n최소 240*320 픽셀 이상이어야 합니다.<'),
]

count = 0
not_found = []
for src, dst in t:
    if src in content:
        content = content.replace(src, dst)
        count += 1
    else:
        not_found.append(src[:60])

with open(file, 'w', encoding='utf-8') as f:
    f.write(content)

print(f'\n완료: {count}/{len(t)}개 항목 번역')
if not_found:
    print(f'\n[미발견 {len(not_found)}개]:')
    for s in not_found:
        print(f'  - {s}')
