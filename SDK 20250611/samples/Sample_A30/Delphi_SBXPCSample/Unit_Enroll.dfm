object frmEnroll: TfrmEnroll
  Left = 261
  Top = 177
  Width = 921
  Height = 505
  Caption = 'Enroll Management'
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  OnClose = FormClose
  OnCreate = FormCreate
  OnShow = FormShow
  PixelsPerInch = 96
  TextHeight = 13
  object Label1: TLabel
    Left = 24
    Top = 64
    Width = 96
    Height = 19
    Caption = 'Enroll Number :'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
  end
  object Label2: TLabel
    Left = 24
    Top = 96
    Width = 92
    Height = 19
    Caption = 'Card Number :'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
  end
  object Label3: TLabel
    Left = 24
    Top = 128
    Width = 122
    Height = 19
    Caption = 'EMachine Number :'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
  end
  object Label4: TLabel
    Left = 24
    Top = 160
    Width = 108
    Height = 19
    Caption = 'Backup Number :'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
  end
  object Label5: TLabel
    Left = 24
    Top = 192
    Width = 58
    Height = 19
    Caption = 'Privilege :'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
  end
  object Label6: TLabel
    Left = 256
    Top = 64
    Width = 44
    Height = 19
    Caption = 'Name :'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
  end
  object lblEnrollData: TLabel
    Left = 256
    Top = 128
    Width = 83
    Height = 19
    Caption = 'Enrolled Data'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
  end
  object lblTotal: TLabel
    Left = 376
    Top = 128
    Width = 41
    Height = 19
    Caption = 'Total : '
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
  end
  object Label7: TLabel
    Left = 24
    Top = 224
    Width = 42
    Height = 19
    Caption = 'Duress'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
  end
  object Label8: TLabel
    Left = 24
    Top = 256
    Width = 59
    Height = 19
    Caption = 'User TZ1'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
  end
  object Label9: TLabel
    Left = 24
    Top = 288
    Width = 59
    Height = 19
    Caption = 'User TZ2'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
  end
  object Label10: TLabel
    Left = 24
    Top = 328
    Width = 72
    Height = 19
    Caption = 'User Group'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
  end
  object Label11: TLabel
    Left = 24
    Top = 384
    Width = 33
    Height = 19
    Caption = 'From'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
  end
  object Label12: TLabel
    Left = 192
    Top = 384
    Width = 16
    Height = 19
    Caption = 'To'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
  end
  object Label14: TLabel
    Left = 24
    Top = 424
    Width = 63
    Height = 19
    Caption = 'ConvType'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
  end
  object lblMessage: TStaticText
    Left = 8
    Top = 16
    Width = 897
    Height = 33
    Align = alCustom
    Alignment = taCenter
    AutoSize = False
    BorderStyle = sbsSunken
    Caption = 'Message'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -19
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 0
  end
  object txtEnrollNumber: TEdit
    Left = 152
    Top = 64
    Width = 89
    Height = 27
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ImeName = 'Korean Input System (IME 2000)'
    ParentFont = False
    TabOrder = 1
    Text = '1'
  end
  object txtCardNumber: TEdit
    Left = 152
    Top = 96
    Width = 89
    Height = 27
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ImeName = 'Korean Input System (IME 2000)'
    ParentFont = False
    TabOrder = 2
  end
  object cmbEMachineNumber: TComboBox
    Left = 152
    Top = 128
    Width = 89
    Height = 27
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ImeName = 'Korean Input System (IME 2000)'
    ItemHeight = 19
    ParentFont = False
    TabOrder = 3
    Text = '1'
    Items.Strings = (
      '1'
      '2'
      '3'
      '4'
      '5'
      '6'
      '7'
      '8'
      '9')
  end
  object cmbBackupNumber: TComboBox
    Left = 152
    Top = 160
    Width = 89
    Height = 27
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ImeName = 'Korean Input System (IME 2000)'
    ItemHeight = 19
    ParentFont = False
    TabOrder = 4
    Text = '0'
    Items.Strings = (
      '0'
      '1'
      '2'
      '10'
      '11'
      '12'
      '13'
      '14')
  end
  object cmbPrivilege: TComboBox
    Left = 152
    Top = 192
    Width = 89
    Height = 27
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ImeName = 'Korean Input System (IME 2000)'
    ItemHeight = 19
    ParentFont = False
    TabOrder = 5
    Text = '0'
    Items.Strings = (
      '0'
      '1')
  end
  object txtName: TEdit
    Left = 312
    Top = 64
    Width = 161
    Height = 24
    Font.Charset = GB2312_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = '??'
    Font.Style = []
    ImeMode = imChinese
    ImeName = 'Korean Input System (IME 2000)'
    ParentFont = False
    TabOrder = 6
  end
  object chkDisable: TCheckBox
    Left = 256
    Top = 104
    Width = 97
    Height = 17
    Caption = 'Disable'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 7
  end
  object lstEnrollData: TListBox
    Left = 256
    Top = 152
    Width = 217
    Height = 153
    ImeName = 'Korean Input System (IME 2000)'
    ItemHeight = 13
    TabOrder = 8
  end
  object cmdDel: TButton
    Left = 392
    Top = 315
    Width = 83
    Height = 30
    Caption = 'Delete DB'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 9
    OnClick = cmdDelClick
  end
  object cmdGetEnrollData: TButton
    Left = 488
    Top = 58
    Width = 201
    Height = 30
    Caption = 'Get Enroll Data'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 10
    OnClick = cmdGetEnrollDataClick
  end
  object cmdSetEnrollData: TButton
    Left = 688
    Top = 58
    Width = 201
    Height = 30
    Caption = 'Set Enroll Data'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 11
    OnClick = cmdSetEnrollDataClick
  end
  object cmdDeleteEnrollData: TButton
    Left = 688
    Top = 90
    Width = 201
    Height = 30
    Caption = 'Delete Enroll Data'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 12
    OnClick = cmdDeleteEnrollDataClick
  end
  object cmdGetAllEnrollData: TButton
    Left = 488
    Top = 122
    Width = 201
    Height = 30
    Caption = 'Get All Enroll Data'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 13
    OnClick = cmdGetAllEnrollDataClick
  end
  object cmdSetAllEnrollData: TButton
    Left = 689
    Top = 122
    Width = 200
    Height = 30
    Caption = 'Set All Enroll Data'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 14
    OnClick = cmdSetAllEnrollDataClick
  end
  object cmdGetName: TButton
    Left = 488
    Top = 158
    Width = 201
    Height = 30
    Caption = 'Get Name Data'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 15
    OnClick = cmdGetNameClick
  end
  object cmdSetName: TButton
    Left = 688
    Top = 158
    Width = 201
    Height = 30
    Caption = 'Set Name Data'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 16
    OnClick = cmdSetNameClick
  end
  object cmdSetCompany: TButton
    Left = 488
    Top = 190
    Width = 201
    Height = 30
    Caption = 'Set Company Name'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 17
    OnClick = cmdSetCompanyClick
  end
  object cmdDeleteCompany: TButton
    Left = 688
    Top = 190
    Width = 201
    Height = 30
    Caption = 'Delete Company Name'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 18
    OnClick = cmdDeleteCompanyClick
  end
  object cmdGetEnrollInfo: TButton
    Left = 489
    Top = 320
    Width = 200
    Height = 30
    Caption = 'Get Enroll Info'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 19
    OnClick = cmdGetEnrollInfoClick
  end
  object cmdEnableUser: TButton
    Left = 688
    Top = 320
    Width = 201
    Height = 30
    Caption = 'Enable User'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 20
    OnClick = cmdEnableUserClick
  end
  object cmdModifyPrivilege: TButton
    Left = 488
    Top = 224
    Width = 201
    Height = 30
    Caption = 'Modify Privilege'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 21
    OnClick = cmdModifyPrivilegeClick
  end
  object cmdEmptyEnrollData: TButton
    Left = 488
    Top = 352
    Width = 201
    Height = 30
    Caption = 'Empty Enroll Data'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 22
    OnClick = cmdEmptyEnrollDataClick
  end
  object cmdClearData: TButton
    Left = 688
    Top = 352
    Width = 201
    Height = 30
    Caption = 'Clear All Data(E,GL,SL)'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 23
    OnClick = cmdClearDataClick
  end
  object cmdExit: TButton
    Left = 568
    Top = 388
    Width = 201
    Height = 30
    Caption = 'Exit'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 24
    OnClick = cmdExitClick
  end
  object cmdModifyDuress: TButton
    Left = 688
    Top = 224
    Width = 201
    Height = 30
    Caption = 'Modify Duress FP'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 25
    OnClick = cmdModifyDuressClick
  end
  object cmbDuress: TComboBox
    Left = 152
    Top = 224
    Width = 89
    Height = 27
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ImeName = 'Korean Input System (IME 2000)'
    ItemHeight = 19
    ParentFont = False
    TabOrder = 26
    Text = '0'
    Items.Strings = (
      '0'
      '1')
  end
  object txtUserTz1: TEdit
    Left = 152
    Top = 256
    Width = 89
    Height = 27
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ImeName = 'Korean Input System (IME 2000)'
    ParentFont = False
    TabOrder = 27
    Text = '0'
  end
  object txtUserTz2: TEdit
    Left = 152
    Top = 288
    Width = 89
    Height = 27
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ImeName = 'Korean Input System (IME 2000)'
    ParentFont = False
    TabOrder = 28
    Text = '0'
  end
  object txtUserGroup: TEdit
    Left = 152
    Top = 328
    Width = 89
    Height = 27
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ImeName = 'Korean Input System (IME 2000)'
    ParentFont = False
    TabOrder = 29
    Text = '0'
  end
  object chkUsePeriod: TCheckBox
    Left = 32
    Top = 360
    Width = 97
    Height = 17
    Caption = 'User Period'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 30
    OnClick = chkUsePeriodClick
  end
  object ComboBox1: TComboBox
    Left = 104
    Top = 424
    Width = 89
    Height = 27
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ImeName = 'Korean Input System (IME 2000)'
    ItemHeight = 19
    ParentFont = False
    TabOrder = 31
    Text = '0'
    Items.Strings = (
      'None'
      'SH'
      'Arm'
      'Arm400')
  end
  object dtValidFrom: TDateTimePicker
    Left = 64
    Top = 384
    Width = 113
    Height = 27
    Date = 40780.430629699080000000
    Time = 40780.430629699080000000
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ImeName = 'Korean Input System (IME 2000)'
    ParentFont = False
    TabOrder = 32
  end
  object dtValidUntil: TDateTimePicker
    Left = 216
    Top = 384
    Width = 113
    Height = 27
    Date = 40780.430629699080000000
    Time = 40780.430629699080000000
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ImeName = 'Korean Input System (IME 2000)'
    ParentFont = False
    TabOrder = 33
  end
  object cmdGetUserInfo: TButton
    Left = 488
    Top = 256
    Width = 201
    Height = 30
    Caption = 'Get User Info'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 34
    OnClick = cmdGetUserInfoClick
  end
  object cmdSetUserInfo: TButton
    Left = 688
    Top = 256
    Width = 201
    Height = 30
    Caption = 'Set User Info'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 35
    OnClick = cmdSetUserInfoClick
  end
  object cmdGetUserPeriod: TButton
    Left = 488
    Top = 288
    Width = 201
    Height = 30
    Caption = 'Get User Period'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 36
    OnClick = cmdGetUserPeriodClick
  end
  object cmdSetUserPeriod: TButton
    Left = 688
    Top = 288
    Width = 201
    Height = 30
    Caption = 'Set User Period'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 37
    OnClick = cmdSetUserPeriodClick
  end
  object con: TADOConnection
    ConnectionString = 
      'Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\WORK2_Drive_Back' +
      'up\_DELPHI_WORK\SB100BPC_Delphi\out\datEnrollDat.mdb;Persist Sec' +
      'urity Info=False'
    LoginPrompt = False
    Provider = 'Microsoft.Jet.OLEDB.4.0'
    Left = 264
    Top = 320
  end
  object tblEnroll: TADOTable
    Connection = con
    TableName = 'tblEnroll'
    Left = 296
    Top = 320
  end
  object ds: TDataSource
    DataSet = tblEnroll
    Left = 336
    Top = 320
  end
end
