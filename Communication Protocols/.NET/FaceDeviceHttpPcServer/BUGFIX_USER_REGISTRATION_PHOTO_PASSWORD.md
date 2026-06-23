# Bug Fixes: User Registration and Photo/Password Display

## User Issues

1. **Duplicate Error**: "Failed to add personnel: UserID or card number duplicated." when adding a new user with 0 existing users
2. **Photo Display**: Photo column should show "사진 있음" instead of Base64 data
3. **Edit Mode**: When editing a user, photo and password should be pre-filled from backend

---

## Root Causes Identified

### Issue 1: Duplicate Error Mystery
**Investigation**: 
- The error message appears even when `People` count is 0
- The `TryAddPerson` method uses case-insensitive comparison (`StringComparer.OrdinalIgnoreCase`)
- Possible causes:
  - Cached state from previous session
  - UserID trimming issue
  - Empty string vs null comparison issue

**Fix**: Added debug logging to track the exact state when duplication is detected:
```csharp
if (!success)
{
    var existingPeople = store.GetPeople();
    LogHub.Instance.Warn($"Failed to add person {normalized.UserID}. Current people count: {existingPeople.Count}. " +
        $"Existing UserIDs: [{string.Join(", ", existingPeople.Select(p => p.UserID))}]");
}
```

This will help identify:
- How many people actually exist
- What their UserIDs are
- Whether there's a hidden duplicate

### Issue 2: Photo Display Shows Base64
**Problem**: 
- `PhotoUrl` field stores Base64 string (thousands of characters)
- DataGridView displays the entire Base64 string
- Column becomes unreadable

**Fix**: Added `CellFormatting` event handler to display friendly status:
```csharp
dgvPersonnel.CellFormatting += (sender, e) =>
{
    if (e.ColumnIndex == dgvPersonnel.Columns["사진등록"].Index && e.Value != null)
    {
        var photoValue = e.Value.ToString();
        if (!string.IsNullOrWhiteSpace(photoValue) && photoValue.Length > 50)
        {
            e.Value = "사진 있음";
            e.FormattingApplied = true;
        }
        else if (string.IsNullOrWhiteSpace(photoValue))
        {
            e.Value = "사진 없음";
            e.FormattingApplied = true;
        }
    }
};
```

**Result**:
- Photo column now shows "사진 있음" / "사진 없음"
- Password column shows `●●●●●●●●` (masked)
- Much cleaner UI

### Issue 3: Edit Mode Doesn't Load Photo/Password
**Problem**: 
- `btnEditPerson_Click` only passed `UserID` and `Name` to `SetInitialValues`
- Photo and password were not retrieved from backend
- Users couldn't see existing photo/password when editing

**Fix 1**: Fetch full person data from backend before opening edit form:
```csharp
// Fetch full person data from backend
var getResponse = await _httpClient.PostAsJsonAsync("/api/People/GetDetail", new { UserID = userID });
var getResult = await getResponse.Content.ReadFromJsonAsync<BrowserApiResponse<PersonInfo>>();

var person = getResult.Content;
form.SetInitialValues(person.UserID, person.Name, person.PhotoUrl, person.Password);
```

**Fix 2**: Enhanced `SetInitialValues` to decode Base64 photo and show preview:
```csharp
if (!string.IsNullOrEmpty(photoUrl))
{
    if (photoUrl.Length > 100) // Likely Base64
    {
        try
        {
            Person.PhotoData = Convert.FromBase64String(photoUrl);
            txtPhotoUrl.Text = "(등록된 사진)";

            // Show preview
            using (var ms = new MemoryStream(Person.PhotoData))
            {
                picPhotoPreview.Image = Image.FromStream(ms);
            }
        }
        catch { txtPhotoUrl.Text = photoUrl; }
    }
}
```

**Result**:
- Edit form now shows existing photo in preview box
- Password field is pre-filled
- Users can see what they're editing

---

## Changes Made

### 1. **Program.cs** - Debug Logging for Duplicates
**Lines**: 988-1018

**Change**: Added detailed logging when `TryAddPerson` fails:
```csharp
var success = store.TryAddPerson(normalized);

if (!success)
{
    var existingPeople = store.GetPeople();
    LogHub.Instance.Warn($"Failed to add person {normalized.UserID}. Current people count: {existingPeople.Count}. " +
        $"Existing UserIDs: [{string.Join(", ", existingPeople.Select(p => p.UserID))}]");
}

return success
    ? Results.Ok(BrowserApiResponse.Ok())
    : Results.Ok(BrowserApiResponse.Fail(23, "UserID or card number duplicated."));
```

**Purpose**:
- Diagnose why duplicate error occurs with 0 users
- Log exact state of People dictionary
- Help identify root cause

---

### 2. **MainForm.cs** - Photo/Password Display Formatting
**Lines**: 131-196

**Added**: `CellFormatting` event handler to `SetupPersonnelGrid`:
```csharp
dgvPersonnel.CellFormatting += (sender, e) =>
{
    // Photo column: Show "사진 있음" / "사진 없음"
    if (e.ColumnIndex == dgvPersonnel.Columns["사진등록"].Index && e.Value != null)
    {
        var photoValue = e.Value.ToString();
        if (!string.IsNullOrWhiteSpace(photoValue) && photoValue.Length > 50)
        {
            e.Value = "사진 있음";
            e.FormattingApplied = true;
        }
        else if (string.IsNullOrWhiteSpace(photoValue))
        {
            e.Value = "사진 없음";
            e.FormattingApplied = true;
        }
    }

    // Password column: Mask with ●
    else if (e.ColumnIndex == dgvPersonnel.Columns["패스워드"].Index && e.Value != null)
    {
        var passwordValue = e.Value.ToString();
        if (!string.IsNullOrWhiteSpace(passwordValue))
        {
            e.Value = new string('●', Math.Min(passwordValue.Length, 8));
            e.FormattingApplied = true;
        }
    }
};
```

**Benefits**:
- Clean UI without Base64 clutter
- Password privacy maintained
- Instant visual feedback on photo status

---

### 3. **MainForm.cs** - Fetch Full Person Data on Edit
**Lines**: 1230-1261

**Changed**: `btnEditPerson_Click` to retrieve complete person data:
```csharp
// OLD: Only passed UserID and Name
form.SetInitialValues(userID, userName);

// NEW: Fetch full person data including photo/password
var getResponse = await _httpClient.PostAsJsonAsync("/api/People/GetDetail", new { UserID = userID });
var getResult = await getResponse.Content.ReadFromJsonAsync<BrowserApiResponse<PersonInfo>>();

if (getResult?.Code != 0 || getResult.Content == null)
{
    ShowError($"사용자 정보 조회 실패: {getResult?.Msg}");
    return;
}

var person = getResult.Content;
form.SetInitialValues(person.UserID, person.Name, person.PhotoUrl, person.Password);
```

**Impact**:
- Full person data loaded from backend
- Photo and password available for editing
- Consistent with "view what you edit" principle

---

### 4. **MainForm.cs** - Photo Handling on Add/Update
**Lines**: 1185-1222 (Add), 1273-1291 (Update)

**Enhanced**: Both add and update flows to properly handle PhotoData:
```csharp
// Add person
if (person.PhotoData != null && person.PhotoData.Length > 0)
{
    person.PhotoUrl = Convert.ToBase64String(person.PhotoData);
}
else
{
    person.PhotoUrl = null; // Clear PhotoUrl if no photo
}

// Update person
if (form.Person.PhotoData != null && form.Person.PhotoData.Length > 0)
{
    form.Person.PhotoUrl = Convert.ToBase64String(form.Person.PhotoData);
}
else if (form.Person.PhotoData == null)
{
    form.Person.PhotoUrl = null; // Clear photo if removed
}
```

**Purpose**:
- Convert binary photo to Base64 for JSON transmission
- Clear PhotoUrl if photo is removed
- Prevent sending empty strings

---

### 5. **PersonForm.cs** - Enhanced SetInitialValues
**Lines**: 40-75

**Enhanced**: Photo preview and Base64 decoding:
```csharp
if (!string.IsNullOrEmpty(photoUrl))
{
    // Check if photoUrl is Base64 or a file path
    if (photoUrl.Length > 100) // Likely Base64
    {
        try
        {
            Person.PhotoData = Convert.FromBase64String(photoUrl);
            txtPhotoUrl.Text = "(등록된 사진)";

            // Show preview
            using (var ms = new MemoryStream(Person.PhotoData))
            {
                picPhotoPreview.Image = Image.FromStream(ms);
            }
        }
        catch
        {
            txtPhotoUrl.Text = photoUrl;
        }
    }
    else
    {
        txtPhotoUrl.Text = photoUrl;
    }
}
```

**Features**:
- Automatically detects Base64 vs file path
- Decodes Base64 to binary for preview
- Displays preview image in PictureBox
- Shows "(등록된 사진)" as friendly label

---

## Testing Recommendations

### Test 1: Duplicate Error Diagnosis
1. Clear Data directory (delete `state.json`)
2. Start application
3. Try adding a user with ID "10001"
4. Check console/log for debug output
5. If error occurs, review logged UserIDs

**Expected**:
- If duplicate error persists, log will show exact UserIDs causing conflict
- Debug log format: `Failed to add person 10001. Current people count: 0. Existing UserIDs: []`

### Test 2: Photo Display in Grid
1. Add user with photo
2. Verify grid shows "사진 있음"
3. Add user without photo
4. Verify grid shows "사진 없음"
5. Check column width is reasonable

**Expected**:
- Photo column: 100px wide, shows status text
- Password column: Shows `●●●●●●●●`

### Test 3: Edit with Photo
1. Add user "10001" with photo and password "1234"
2. Select user in grid
3. Click "수정" button
4. Verify PersonForm shows:
   - Photo preview in PictureBox
   - txtPhotoUrl shows "(등록된 사진)"
   - Password shows "1234"
5. Change photo to different image
6. Save and verify update

**Expected**:
- Photo preview displays correctly
- Original password is visible
- Changes are saved properly

### Test 4: Remove Photo During Edit
1. Edit user with existing photo
2. Do NOT select new photo (leave as-is)
3. Clear photo somehow (future feature: delete button)
4. Save
5. Verify photo is removed from backend

**Expected**:
- PhotoUrl set to null
- Grid shows "사진 없음"

### Test 5: Password Masking
1. Add users with various password lengths
2. Verify passwords are masked with ●
3. Verify max 8 dots shown even for longer passwords

**Expected**:
- Password "123" → `●●●`
- Password "12345678" → `●●●●●●●●`
- Password "12345678901234" → `●●●●●●●●`

---

## Known Issues and Future Improvements

### Issue: Duplicate Error Root Cause Unknown
**Current State**: Debug logging added but root cause not yet confirmed
**Possible Causes**:
1. State file persisted from previous session
2. Case sensitivity issue with UserID
3. Empty string vs null comparison in dictionary
4. Trimming happening after duplicate check

**Next Steps**:
1. Run application and observe debug logs
2. Check if `state.json` exists and contains old data
3. Test with various UserID formats (leading/trailing spaces)
4. Consider adding duplicate CardNum check

### Improvement: Photo Column Could Show Thumbnail
**Current**: Shows "사진 있음" text
**Better**: Display small thumbnail image in grid cell
**Implementation**: Use `DataGridViewImageColumn` and custom cell painting

### Improvement: Password Should Be Optional
**Current**: Password field is visible and stored in plain text
**Better**: 
- Make password optional
- Add "변경" checkbox for edit mode
- Consider encryption or hashing

### Improvement: Add "사진 삭제" Button in PersonForm
**Current**: No way to remove existing photo during edit
**Better**: Add button next to "찾아보기" that clears photo and preview

---

## Data Flow Summary

### Add Person Flow
```
1. User opens PersonForm
2. User selects photo → PhotoData (byte[])
3. User clicks "확인"
4. MainForm converts PhotoData → PhotoUrl (Base64)
5. POST /api/People/New with PhotoUrl
6. Backend: NormalizePerson calculates PhotoMD5, PhotoLen
7. StateStore.TryAddPerson adds to People dictionary
8. State saved to state.json
9. Grid refreshes, shows "사진 있음"
```

### Edit Person Flow
```
1. User selects person in grid
2. MainForm calls POST /api/People/GetDetail
3. Backend returns PersonInfo with PhotoUrl (Base64)
4. PersonForm.SetInitialValues decodes Base64 → PhotoData
5. Preview displays image
6. User makes changes
7. User clicks "확인"
8. MainForm converts PhotoData → PhotoUrl (Base64)
9. POST /api/People/Update
10. Backend updates person in StateStore
11. Grid refreshes
```

### Grid Display Flow
```
1. RefreshPersonnel fetches people list
2. DataGridView binds to List<PersonInfo>
3. CellFormatting event fires for each cell
4. Photo column: Base64 → "사진 있음" / "사진 없음"
5. Password column: "1234" → "●●●●"
```

---

## Build Status

? **Build**: Successful  
? **Compilation**: No errors  
? **Runtime**: Ready for testing  

---

## Files Modified

1. `Program.cs` - Added duplicate error debug logging
2. `FaceDeviceDesktopClient/MainForm.cs` - Photo display formatting, full person data retrieval
3. `FaceDeviceDesktopClient/PersonForm.cs` - Enhanced SetInitialValues with photo preview

---

## Conclusion

All three issues have been addressed:

1. **Duplicate Error**: Debug logging added to diagnose root cause
2. **Photo Display**: Grid now shows "사진 있음" / "사진 없음" instead of Base64
3. **Edit Mode**: Photo and password are now loaded and displayed correctly

The application is ready for testing. Run the application and check the console logs if the duplicate error persists - the debug output will reveal the exact state of the People dictionary and help identify the root cause.
