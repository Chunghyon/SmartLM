object frmEnroll: TfrmEnroll
  Left = 456
  Top = 146
  Width = 480
  Height = 656
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
    Width = 84
    Height = 19
    Caption = 'Car Number :'
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
    Left = 24
    Top = 224
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
    Left = 24
    Top = 288
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
    Left = 144
    Top = 288
    Width = 42
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
    Left = 16
    Top = 544
    Width = 59
    Height = 19
    Caption = 'User Tz1:'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
  end
  object Label8: TLabel
    Left = 16
    Top = 576
    Width = 59
    Height = 19
    Caption = 'User Tz2:'
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
    Width = 449
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
      '14'
      '')
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
    ItemHeight = 19
    ParentFont = False
    TabOrder = 5
    Text = '0'
    Items.Strings = (
      '0'
      '1'
      '2')
  end
  object txtName: TEdit
    Left = 80
    Top = 224
    Width = 161
    Height = 27
    Font.Charset = GB2312_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = '??'
    Font.Style = []
    ImeMode = imChinese
    ParentFont = False
    TabOrder = 6
  end
  object chkEnable: TCheckBox
    Left = 24
    Top = 264
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
    Left = 24
    Top = 312
    Width = 217
    Height = 153
    ItemHeight = 13
    TabOrder = 8
  end
  object cmdDel: TButton
    Left = 160
    Top = 475
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
    Left = 256
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
    Left = 256
    Top = 90
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
    Left = 256
    Top = 122
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
    Left = 256
    Top = 154
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
    Left = 257
    Top = 186
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
    Left = 256
    Top = 238
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
    Left = 256
    Top = 270
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
    Left = 256
    Top = 302
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
    Left = 256
    Top = 334
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
    Left = 257
    Top = 408
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
    Left = 256
    Top = 440
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
    Left = 256
    Top = 472
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
    Left = 256
    Top = 504
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
    Left = 256
    Top = 536
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
    Left = 256
    Top = 572
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
  object txtUserTz1: TEdit
    Left = 80
    Top = 544
    Width = 97
    Height = 27
    Font.Charset = GB2312_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = '??'
    Font.Style = []
    ImeMode = imChinese
    ParentFont = False
    TabOrder = 25
  end
  object txtUserTz2: TEdit
    Left = 80
    Top = 576
    Width = 97
    Height = 27
    Font.Charset = GB2312_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = '??'
    Font.Style = []
    ImeMode = imChinese
    ParentFont = False
    TabOrder = 26
  end
  object con: TADOConnection
    ConnectionString = 
      'Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\WORK2_Drive_Back' +
      'up\_DELPHI_WORK\SB100BPC_Delphi\out\datEnrollDat.mdb;Persist Sec' +
      'urity Info=False'
    LoginPrompt = False
    Provider = 'Microsoft.Jet.OLEDB.4.0'
    Left = 32
    Top = 480
  end
  object tblEnroll: TADOTable
    Connection = con
    TableName = 'tblEnroll'
    Left = 64
    Top = 480
  end
  object ds: TDataSource
    DataSet = tblEnroll
    Left = 104
    Top = 480
  end
end
