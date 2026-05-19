object frmLockCtrl: TfrmLockCtrl
  Left = 576
  Top = 150
  Width = 553
  Height = 744
  Caption = 'Lock Control'
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  OnClose = FormClose
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 13
  object Label9: TLabel
    Left = 32
    Top = 504
    Width = 110
    Height = 19
    Caption = 'Unlock Group 1:'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
  end
  object Label18: TLabel
    Left = 128
    Top = 64
    Width = 97
    Height = 19
    Caption = 'Door Number:'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
  end
  object lblMessage: TStaticText
    Left = 8
    Top = 10
    Width = 513
    Height = 31
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
  object cmdGetDoorStatus: TButton
    Left = 392
    Top = 112
    Width = 129
    Height = 30
    Caption = 'Get DoorStatus'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 1
    OnClick = cmdGetDoorStatusClick
  end
  object cmdDoorOpen: TButton
    Left = 392
    Top = 152
    Width = 129
    Height = 30
    Caption = 'Door Open'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 2
    OnClick = cmdDoorOpenClick
  end
  object cmdUncondOpen: TButton
    Left = 392
    Top = 232
    Width = 129
    Height = 30
    Caption = 'Uncond Open'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 3
    OnClick = cmdUncondOpenClick
  end
  object cmdAutoRecover: TButton
    Left = 392
    Top = 192
    Width = 129
    Height = 30
    Caption = 'Auto Recover'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 4
    OnClick = cmdAutoRecoverClick
  end
  object cmdUncondClose: TButton
    Left = 392
    Top = 272
    Width = 129
    Height = 30
    Caption = 'Uncond Close'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 5
    OnClick = cmdUncondCloseClick
  end
  object cmdRestart: TButton
    Left = 392
    Top = 352
    Width = 129
    Height = 30
    Caption = 'Reboot'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 6
    OnClick = cmdRestartClick
  end
  object cmdWarnCancel: TButton
    Left = 392
    Top = 312
    Width = 129
    Height = 30
    Caption = 'Warn Cancel'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 7
    OnClick = cmdWarnCancelClick
  end
  object GroupBox1: TGroupBox
    Left = 16
    Top = 96
    Width = 361
    Height = 297
    Caption = 'Door Setting'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -19
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 8
    object Label1: TLabel
      Left = 16
      Top = 40
      Width = 122
      Height = 19
      Caption = 'DoorSensor Type:'
      Font.Charset = ANSI_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = [fsBold]
      ParentFont = False
    end
    object Label2: TLabel
      Left = 16
      Top = 72
      Width = 136
      Height = 19
      Caption = 'Lock Release Time:'
      Font.Charset = ANSI_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = [fsBold]
      ParentFont = False
    end
    object Label3: TLabel
      Left = 16
      Top = 112
      Width = 138
      Height = 19
      Caption = 'Door Open Timeout:'
      Font.Charset = ANSI_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = [fsBold]
      ParentFont = False
    end
    object Label4: TLabel
      Left = 16
      Top = 144
      Width = 92
      Height = 19
      Caption = 'Use Antipass:'
      Font.Charset = ANSI_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = [fsBold]
      ParentFont = False
    end
    object Label5: TLabel
      Left = 16
      Top = 176
      Width = 87
      Height = 19
      Caption = 'Antipass No:'
      Font.Charset = ANSI_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = [fsBold]
      ParentFont = False
    end
    object Label6: TLabel
      Left = 16
      Top = 208
      Width = 125
      Height = 19
      Caption = 'Antipass Location:'
      Font.Charset = ANSI_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = [fsBold]
      ParentFont = False
    end
    object btnReadDoorSetting: TButton
      Left = 56
      Top = 248
      Width = 97
      Height = 33
      Caption = 'Read'
      TabOrder = 0
      OnClick = btnReadDoorSettingClick
    end
    object btnWriteDoorSetting: TButton
      Left = 176
      Top = 248
      Width = 97
      Height = 33
      Caption = 'Write'
      TabOrder = 1
      OnClick = btnWriteDoorSettingClick
    end
    object cmbDoorSensorType: TComboBox
      Left = 192
      Top = 40
      Width = 145
      Height = 27
      Font.Charset = ANSI_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = [fsBold]
      ItemHeight = 19
      ItemIndex = 0
      ParentFont = False
      TabOrder = 2
      Text = 'No'
      Items.Strings = (
        'No'
        'N.O.'
        'N.C.')
    end
    object cmbAntipassUsage: TComboBox
      Left = 192
      Top = 136
      Width = 145
      Height = 27
      Font.Charset = ANSI_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = [fsBold]
      ItemHeight = 19
      ItemIndex = 0
      ParentFont = False
      TabOrder = 3
      Text = 'Use'
      Items.Strings = (
        'Use'
        'No Use')
    end
    object cmbAntipassNo: TComboBox
      Left = 192
      Top = 168
      Width = 145
      Height = 27
      Font.Charset = ANSI_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = [fsBold]
      ItemHeight = 19
      ItemIndex = 0
      ParentFont = False
      TabOrder = 4
      Text = 'Group 1'
      Items.Strings = (
        'Group 1'
        'Group 2')
    end
    object cmbAntipassLocation: TComboBox
      Left = 192
      Top = 200
      Width = 145
      Height = 27
      Font.Charset = ANSI_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = [fsBold]
      ItemHeight = 19
      ItemIndex = 0
      ParentFont = False
      TabOrder = 5
      Text = 'Master'
      Items.Strings = (
        'Master'
        'Slave')
    end
    object txtLockReleaseTime: TEdit
      Left = 192
      Top = 72
      Width = 145
      Height = 27
      Font.Charset = ANSI_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = [fsBold]
      ParentFont = False
      TabOrder = 6
    end
    object txtDoorOpenTimeout: TEdit
      Left = 192
      Top = 104
      Width = 145
      Height = 27
      Font.Charset = ANSI_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = [fsBold]
      ParentFont = False
      TabOrder = 7
    end
  end
  object GroupBox2: TGroupBox
    Left = 16
    Top = 400
    Width = 505
    Height = 289
    Caption = 'Unlock Setting'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -19
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
    TabOrder = 9
    object Label7: TLabel
      Left = 16
      Top = 40
      Width = 110
      Height = 19
      Caption = 'Unlock Group 1:'
      Font.Charset = ANSI_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = [fsBold]
      ParentFont = False
    end
    object Label8: TLabel
      Left = 16
      Top = 80
      Width = 110
      Height = 19
      Caption = 'Unlock Group 2:'
      Font.Charset = ANSI_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = [fsBold]
      ParentFont = False
    end
    object Label10: TLabel
      Left = 16
      Top = 120
      Width = 110
      Height = 19
      Caption = 'Unlock Group 3:'
      Font.Charset = ANSI_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = [fsBold]
      ParentFont = False
    end
    object Label11: TLabel
      Left = 16
      Top = 160
      Width = 110
      Height = 19
      Caption = 'Unlock Group 4:'
      Font.Charset = ANSI_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = [fsBold]
      ParentFont = False
    end
    object Label12: TLabel
      Left = 16
      Top = 200
      Width = 110
      Height = 19
      Caption = 'Unlock Group 5:'
      Font.Charset = ANSI_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = [fsBold]
      ParentFont = False
    end
    object Label13: TLabel
      Left = 264
      Top = 40
      Width = 110
      Height = 19
      Caption = 'Unlock Group 6:'
      Font.Charset = ANSI_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = [fsBold]
      ParentFont = False
    end
    object Label14: TLabel
      Left = 264
      Top = 80
      Width = 110
      Height = 19
      Caption = 'Unlock Group 7:'
      Font.Charset = ANSI_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = [fsBold]
      ParentFont = False
    end
    object Label15: TLabel
      Left = 264
      Top = 120
      Width = 110
      Height = 19
      Caption = 'Unlock Group 8:'
      Font.Charset = ANSI_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = [fsBold]
      ParentFont = False
    end
    object Label16: TLabel
      Left = 264
      Top = 160
      Width = 110
      Height = 19
      Caption = 'Unlock Group 9:'
      Font.Charset = ANSI_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = [fsBold]
      ParentFont = False
    end
    object Label17: TLabel
      Left = 264
      Top = 200
      Width = 118
      Height = 19
      Caption = 'Unlock Group 10:'
      Font.Charset = ANSI_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = [fsBold]
      ParentFont = False
    end
    object txtUnlockGroup0: TEdit
      Left = 152
      Top = 32
      Width = 41
      Height = 29
      TabOrder = 0
    end
    object txtUnlockGroup1: TEdit
      Left = 200
      Top = 32
      Width = 41
      Height = 29
      TabOrder = 1
    end
    object txtUnlockGroup2: TEdit
      Left = 152
      Top = 72
      Width = 41
      Height = 29
      TabOrder = 2
    end
    object txtUnlockGroup3: TEdit
      Left = 200
      Top = 72
      Width = 41
      Height = 29
      TabOrder = 3
    end
    object txtUnlockGroup4: TEdit
      Left = 152
      Top = 112
      Width = 41
      Height = 29
      TabOrder = 4
    end
    object txtUnlockGroup5: TEdit
      Left = 200
      Top = 112
      Width = 41
      Height = 29
      TabOrder = 5
    end
    object txtUnlockGroup6: TEdit
      Left = 152
      Top = 152
      Width = 41
      Height = 29
      TabOrder = 6
    end
    object txtUnlockGroup7: TEdit
      Left = 200
      Top = 152
      Width = 41
      Height = 29
      TabOrder = 7
    end
    object txtUnlockGroup8: TEdit
      Left = 152
      Top = 192
      Width = 41
      Height = 29
      TabOrder = 8
    end
    object txtUnlockGroup9: TEdit
      Left = 200
      Top = 192
      Width = 41
      Height = 29
      TabOrder = 9
    end
    object txtUnlockGroup10: TEdit
      Left = 400
      Top = 32
      Width = 41
      Height = 29
      TabOrder = 10
    end
    object txtUnlockGroup11: TEdit
      Left = 448
      Top = 32
      Width = 41
      Height = 29
      TabOrder = 11
    end
    object txtUnlockGroup12: TEdit
      Left = 400
      Top = 72
      Width = 41
      Height = 29
      TabOrder = 12
    end
    object txtUnlockGroup13: TEdit
      Left = 448
      Top = 72
      Width = 41
      Height = 29
      TabOrder = 13
    end
    object txtUnlockGroup14: TEdit
      Left = 400
      Top = 112
      Width = 41
      Height = 29
      TabOrder = 14
    end
    object txtUnlockGroup15: TEdit
      Left = 448
      Top = 112
      Width = 41
      Height = 29
      TabOrder = 15
    end
    object txtUnlockGroup16: TEdit
      Left = 400
      Top = 152
      Width = 41
      Height = 29
      TabOrder = 16
    end
    object txtUnlockGroup17: TEdit
      Left = 448
      Top = 152
      Width = 41
      Height = 29
      TabOrder = 17
    end
    object txtUnlockGroup18: TEdit
      Left = 400
      Top = 192
      Width = 41
      Height = 29
      TabOrder = 18
    end
    object txtUnlockGroup19: TEdit
      Left = 448
      Top = 192
      Width = 41
      Height = 29
      TabOrder = 19
    end
    object Button3: TButton
      Left = 144
      Top = 240
      Width = 113
      Height = 33
      Caption = 'Read'
      TabOrder = 20
      OnClick = Button3Click
    end
    object Button4: TButton
      Left = 296
      Top = 240
      Width = 113
      Height = 33
      Caption = 'Write'
      TabOrder = 21
      OnClick = Button4Click
    end
  end
  object txtDoorNumber: TEdit
    Left = 248
    Top = 56
    Width = 97
    Height = 29
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -19
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
    TabOrder = 10
    Text = '0'
  end
end
