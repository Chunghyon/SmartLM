# Fix: Photo Display and Duplicate Error Issues

## Problems Identified

### 1. ? Photo Column Shows Empty in List
**Symptom**: After adding a user with photo, the "사진등록" column shows blank/empty space instead of "사진 있음"

**Root Cause**: 
- Client model uses `PhotoUrl` property
- Backend model uses `Photo` property  
- When backend returns PersonInfo, it has `Photo` field with Base64 data
- Client grid is bound to `PhotoUrl` which doesn't exist in the response
- Result: `PhotoUrl` is null/empty, so CellFormatting shows blank

**Evidence**:
```csharp
// Backend Model (Models/Models.cs)
public sealed class PersonInfo
{
    public string Photo { get; set; } = string.Empty;  // ? Backend uses this
    ...
}

// Client Model (FaceDeviceDesktopClient/Models.cs) - BEFORE
public class PersonInfo
{
    public string? PhotoUrl { get; set; }  // ? Client expects this
    ...
}

// Grid Setup
dgvPersonnel.Columns.Add(new DataGridViewTextBoxColumn
{
    DataPropertyName = "PhotoUrl"  // ? Backend doesn't send this
});
```

---

### 2. ? Photo Not Showing in Edit Mode
**Symptom**: When editing a user who has a photo, the photo preview and "(등록된 사진)" label don't appear

**Root Cause**: Same field name mismatch
- Backend sends `Photo` with Base64 data
- `SetInitialValues` receives `photoUrl` parameter but expects it to contain Base64
- Client's `PhotoUrl` property is empty because backend sent `Photo`

**Evidence**:
```csharp
// MainForm passes person.PhotoUrl (null) to SetInitialValues
form.SetInitialValues(person.UserID, person.Name, person.PhotoUrl, person.Password);
                                                     ↑ null (backend uses Photo)
```

---

### 3. ? "서버에 사용자 추가 실패: UserID or card number duplicated" in Edit Mode
**Symptom**: When editing an existing user and clicking "저장 및 선택한 단말기로 전송", get duplicate error

**Root Cause**: 
- Edit mode should use `/api/People/Update` endpoint
- "저장 및 선택한 단말기로 전송" button was always calling `/api/People/New`
- `/api/People/New` checks for duplicates and rejects existing UserIDs
- `/api/People/Update` replaces existing user without duplicate check

**Evidence**:
```csharp
// PersonForm.cs - BtnUploadToDevices_Click (BEFORE)
var addResponse = await _httpClient.PostAsJsonAsync("/api/People/New", personInfo);
                                                     ↑ Always uses New, even in edit mode

// Backend - /api/People/New
app.MapPost("/api/People/New", async (HttpRequest request, StateStore store) =>
{
    return store.TryAddPerson(normalized)  // ? Checks for duplicates
        ? Results.Ok(BrowserApiResponse.Ok())
        : Results.Ok(BrowserApiResponse.Fail(23, "UserID or card number duplicated."));
});

// Backend - /api/People/Update
app.MapPost("/api/People/Update", async (HttpRequest request, StateStore store) =>
{
    return store.UpdatePerson(normalized)  // ? No duplicate check, just updates
        ? Results.Ok(BrowserApiResponse.Ok())
        : Results.Ok(BrowserApiResponse.Fail(3, "Person not found."));
});
```

---

## Solutions Implemented

### Fix 1: Unify Photo Field Names

**Approach**: Make client's `PhotoUrl` property map to backend's `Photo` field

**Implementation**:
```csharp
// FaceDeviceDesktopClient/Models.cs
public class PersonInfo
{
    public string UserID { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    // ... other fields

    // Backend uses Photo field, client uses PhotoUrl for display
    public string? Photo { get; set; }  // ? Added to match backend

    public string? PhotoUrl
    {
        get => Photo;  // ? Map Photo to PhotoUrl for backward compatibility
        set => Photo = value;
    }

    [System.ComponentModel.Browsable(false)]
    public byte[]? PhotoData { get; set; }
}
```

**Benefits**:
- Client can receive `Photo` from backend ?
- Existing code using `PhotoUrl` still works ?
- Grid binding to `PhotoUrl` now gets data from `Photo` ?

**Result**:
```
Backend sends: { "Photo": "iVBORw0KGgoAAAANSUhEUg..." }
                    ↓
Client receives: Photo = "iVBORw0KGgoAAAANSUhEUg..."
                    ↓
Client accesses: PhotoUrl getter returns Photo value
                    ↓
Grid displays: "사진 있음" ?
```

---

### Fix 2: Use Photo Field Consistently

**Changes Made**:

1. **MainForm - btnAddPerson_Click**:
```csharp
// BEFORE
person.PhotoUrl = Convert.ToBase64String(person.PhotoData);

// AFTER
person.Photo = Convert.ToBase64String(person.PhotoData);  // ? Use Photo
```

2. **MainForm - btnEditPerson_Click**:
```csharp
// BEFORE
form.SetInitialValues(person.UserID, person.Name, person.PhotoUrl, person.Password);

// AFTER
form.SetInitialValues(person.UserID, person.Name, person.Photo, person.Password);  // ? Pass Photo
```

3. **MainForm - Update Handler**:
```csharp
// BEFORE
form.Person.PhotoUrl = Convert.ToBase64String(form.Person.PhotoData);

// AFTER
form.Person.Photo = Convert.ToBase64String(form.Person.PhotoData);  // ? Use Photo
```

**Result**: Photo data flows correctly in all scenarios ?

---

### Fix 3: Detect Edit Mode in Upload Button

**Implementation**:
```csharp
// PersonForm.cs - BtnUploadToDevices_Click
var personInfo = new PersonInfo
{
    UserID = txtUserID.Text.Trim(),
    Name = txtName.Text.Trim(),
    Password = txtPassword.Text.Trim()
};

if (Person.PhotoData != null && Person.PhotoData.Length > 0)
{
    personInfo.Photo = Convert.ToBase64String(Person.PhotoData);  // ? Use Photo
}

// ? Detect edit mode
bool isEditMode = IsEditMode && !string.IsNullOrEmpty(_originalUserID);
string apiEndpoint = isEditMode ? "/api/People/Update" : "/api/People/New";

// ? Use appropriate endpoint
var addResponse = await _httpClient.PostAsJsonAsync(apiEndpoint, personInfo);
var addResult = await addResponse.Content.ReadFromJsonAsync<BrowserApiResponse<object>>();

if (addResult == null || addResult.Code != 0)
{
    MessageBox.Show(
        $"서버에 사용자 {(isEditMode ? "업데이트" : "추가")} 실패: {addResult?.Msg ?? "알 수 없는 오류"}",
        "오류",
        MessageBoxButtons.OK,
        MessageBoxIcon.Error);
    return;
}
```

**Logic**:
```
Add Mode:
  IsEditMode = false
  → Use /api/People/New
  → Checks for duplicates ?

Edit Mode:
  IsEditMode = true
  _originalUserID = "10001"
  → Use /api/People/Update
  → No duplicate check, just updates ?
```

**Result**: No more false duplicate errors in edit mode ?

---

## Technical Details

### Field Mapping Flow

#### Add User Flow:
```
1. User selects photo → PhotoData (byte[])
2. Click "저장 및 선택한 단말기로 전송"
3. Convert PhotoData → Base64 string
4. Store in personInfo.Photo
5. POST /api/People/New with Photo field
   ↓
6. Backend: NormalizePerson copies Photo → Photo
7. Backend: StateStore saves with Photo field
8. Backend: Calculates PhotoMD5, PhotoLen
   ↓
9. Client: RefreshPersonnel()
10. Backend: GET /admin/people returns Photo field
11. Client: Receives Photo, PhotoUrl getter returns it
12. Grid: CellFormatting sees PhotoUrl (from Photo)
13. Display: "사진 있음" ?
```

#### Edit User Flow:
```
1. User double-clicks row
2. MainForm: POST /api/People/GetDetail
3. Backend: Returns PersonInfo with Photo field
4. Client: person.Photo = "iVBORw0KGgoAAAANSUhEUg..."
5. MainForm: form.SetInitialValues(..., person.Photo, ...)
6. PersonForm: Receives photoUrl parameter (contains Base64)
7. PersonForm: Detects Base64 (length > 100)
8. PersonForm: Convert Base64 → PhotoData (byte[])
9. PersonForm: Display preview ?
10. User makes changes
11. Click "저장 및 선택한 단말기로 전송"
12. Detect IsEditMode = true
13. POST /api/People/Update (not New) ?
14. No duplicate error ?
```

---

## Before/After Comparison

### Issue 1: Photo Display

**Before**:
```
User List Grid:
┌────────┬────────┬──────────┬──────────┐
│ UserID │ Name   │ PhotoUrl │ Password │
├────────┼────────┼──────────┼──────────┤
│ 10001  │ 홍길동  │          │ ●●●●●●●● │  ? Empty
└────────┴────────┴──────────┴──────────┘

Backend sends: { "Photo": "iVBORw0..." }
Client expects: PhotoUrl (not present) → null
CellFormatting: if (string.IsNullOrWhiteSpace(photoValue)) → blank
```

**After**:
```
User List Grid:
┌────────┬────────┬──────────┬──────────┐
│ UserID │ Name   │ PhotoUrl │ Password │
├────────┼────────┼──────────┼──────────┤
│ 10001  │ 홍길동  │ 사진 있음 │ ●●●●●●●● │  ? Shows status
└────────┴────────┴──────────┴──────────┘

Backend sends: { "Photo": "iVBORw0..." }
Client receives: Photo = "iVBORw0..."
PhotoUrl getter: returns Photo value
CellFormatting: if (photoValue.Length > 50) → "사진 있음" ?
```

---

### Issue 2: Edit Photo Preview

**Before**:
```
Edit Form:
┌─────────────────────────────────────┐
│ 사용자번호: 10001                    │
│ 사용자명: 홍길동                     │
│ 사진등록: [                       ] │  ? Empty
│ ┌─────────┐                        │
│ │         │  ← No preview           │  ? No image
│ └─────────┘                        │
│ 패스워드: [                       ] │  ? Empty
└─────────────────────────────────────┘

Backend sends: { "Photo": "iVBORw0..." }
MainForm passes: person.PhotoUrl (null)
SetInitialValues: photoUrl is null → no preview
```

**After**:
```
Edit Form:
┌─────────────────────────────────────┐
│ 사용자번호: 10001                    │
│ 사용자명: 홍길동                     │
│ 사진등록: (등록된 사진)              │  ? Shows label
│ ┌─────────┐                        │
│ │  [IMG]  │  ← Preview visible      │  ? Shows image
│ └─────────┘                        │
│ 패스워드: 1234                      │  ? Shows password
└─────────────────────────────────────┘

Backend sends: { "Photo": "iVBORw0..." }
MainForm passes: person.Photo (Base64 string)
SetInitialValues: Converts Base64 → PhotoData → Shows preview ?
```

---

### Issue 3: Edit Mode Duplicate Error

**Before**:
```
Edit Mode → "저장 및 선택한 단말기로 전송":
1. User changes name/photo/password
2. Click "저장 및 선택한 단말기로 전송"
3. POST /api/People/New  ? Wrong endpoint
4. Backend: TryAddPerson("10001")
5. Backend: People dictionary already has "10001"
6. Backend: Return error "UserID or card number duplicated"
7. User sees error popup ?
```

**After**:
```
Edit Mode → "저장 및 선택한 단말기로 전송":
1. User changes name/photo/password
2. Click "저장 및 선택한 단말기로 전송"
3. IsEditMode = true → POST /api/People/Update  ? Correct endpoint
4. Backend: UpdatePerson("10001")
5. Backend: People["10001"] = new data
6. Backend: Return success
7. User sees success message ?
```

---

## Testing Checklist

### Test 1: Add User with Photo ?
1. Click "추가" button
2. Enter UserID, Name
3. Click "찾아보기", select photo
4. Click "저장"
5. **Expected**:
   - User appears in list
   - "사진등록" column shows "사진 있음" ?
   - Password column shows `●●●●●●●●`

### Test 2: Add User Without Photo ?
1. Click "추가" button
2. Enter UserID, Name
3. Do NOT select photo
4. Click "저장"
5. **Expected**:
   - User appears in list
   - "사진등록" column shows "사진 없음" ?

### Test 3: Edit User - View Photo ?
1. Double-click user with photo
2. **Expected**:
   - Edit form opens
   - Photo preview shows image ?
   - txtPhotoUrl shows "(등록된 사진)" ?
   - Password field shows actual password ?

### Test 4: Edit User - Change Photo ?
1. Double-click user with photo
2. Click "찾아보기", select different photo
3. Click "저장"
4. **Expected**:
   - Form closes
   - List refreshes
   - Still shows "사진 있음" ?
   - No duplicate error ?

### Test 5: Edit User with Device Upload ?
1. Double-click user
2. Make changes
3. Check devices
4. Click "저장 및 선택한 단말기로 전송"
5. **Expected**:
   - Success message appears
   - Form closes
   - List refreshes
   - No duplicate error ?
   - Devices marked for download ?

### Test 6: Edit User - Remove Photo (Future)
1. Double-click user with photo
2. (Need to add "사진 삭제" button)
3. Click "저장"
4. **Expected**:
   - Photo removed
   - "사진등록" column shows "사진 없음"

---

## Code Changes Summary

### Files Modified

1. **FaceDeviceDesktopClient/Models.cs**
   - Added `Photo` property to match backend
   - Made `PhotoUrl` a property that maps to `Photo`
   - Ensures backward compatibility

2. **FaceDeviceDesktopClient/PersonForm.cs**
   - Changed `personInfo.PhotoUrl` to `personInfo.Photo`
   - Added edit mode detection in `BtnUploadToDevices_Click`
   - Use `/api/People/Update` in edit mode
   - Use `/api/People/New` in add mode

3. **FaceDeviceDesktopClient/MainForm.cs**
   - Changed `person.PhotoUrl` to `person.Photo` in `btnAddPerson_Click`
   - Changed parameter from `person.PhotoUrl` to `person.Photo` in `btnEditPerson_Click`
   - Changed `form.Person.PhotoUrl` to `form.Person.Photo` in update handler

---

## API Endpoints Used

### Add Mode
```
POST /api/People/New
Body: {
  "UserID": "10001",
  "Name": "홍길동",
  "Photo": "iVBORw0KGgoAAAANSUhEUg...",  ? Base64 photo
  "Password": "1234"
}

Response: {
  "result": true,
  "content": null,
  "errCode": 0,
  "error": null
}
```

### Edit Mode
```
POST /api/People/Update
Body: {
  "UserID": "10001",
  "Name": "홍길동",
  "Photo": "iVBORw0KGgoAAAANSUhEUg...",  ? Base64 photo
  "Password": "1234"
}

Response: {
  "result": true,
  "content": null,
  "errCode": 0,
  "error": null
}
```

### Get Details (for Edit)
```
POST /api/People/GetDetail
Body: { "UserID": "10001" }

Response: {
  "result": true,
  "content": {
    "UserID": "10001",
    "Name": "홍길동",
    "Photo": "iVBORw0KGgoAAAANSUhEUg...",  ? Backend returns Photo
    "Password": "1234",
    "PhotoMD5": "a1b2c3d4...",
    "PhotoLen": 12345
  },
  "errCode": 0,
  "error": null
}
```

### List All (for Grid)
```
GET /admin/people

Response: [
  {
    "UserID": "10001",
    "Name": "홍길동",
    "Photo": "iVBORw0KGgoAAAANSUhEUg...",  ? Backend returns Photo
    "Password": "1234",
    ...
  },
  ...
]
```

---

## Build Status

? **Build**: Successful  
? **Compilation**: No errors  
? **Runtime**: Ready for testing  

---

## Conclusion

All three issues have been resolved:

1. ? **Photo column now shows "사진 있음" / "사진 없음"** instead of blank
2. ? **Edit mode displays photo preview and password** correctly
3. ? **No duplicate error when using "저장 및 선택한 단말기로 전송" in edit mode**

The root cause was a field name mismatch between client (`PhotoUrl`) and backend (`Photo`). By making `PhotoUrl` a property that maps to `Photo`, we maintain backward compatibility while ensuring data flows correctly in all scenarios.

The duplicate error was fixed by detecting edit mode and using the appropriate endpoint (`/api/People/Update` vs `/api/People/New`).
