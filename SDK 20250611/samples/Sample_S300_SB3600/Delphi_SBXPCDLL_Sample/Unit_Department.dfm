object frmDepartment: TfrmDepartment
  Left = 305
  Top = 171
  Width = 645
  Height = 628
  Caption = 'frmDepartment'
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  OnClose = FormClose
  OnShow = FormShow
  PixelsPerInch = 96
  TextHeight = 13
  object Label3: TLabel
    Left = 32
    Top = 72
    Width = 81
    Height = 19
    Caption = 'Verify Mode:'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
  end
  object lstDepartment: TListBox
    Left = 16
    Top = 112
    Width = 481
    Height = 465
    ImeName = 'Korean Input System (IME 2000)'
    ItemHeight = 13
    TabOrder = 0
    OnClick = lstDepartmentClick
  end
  object cmdUpdate: TButton
    Left = 512
    Top = 112
    Width = 113
    Height = 41
    Caption = 'Update'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 1
    OnClick = cmdUpdateClick
  end
  object cmdRead: TButton
    Left = 504
    Top = 440
    Width = 113
    Height = 41
    Caption = 'Read'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 2
    OnClick = cmdReadClick
  end
  object cmdWrite: TButton
    Left = 504
    Top = 488
    Width = 113
    Height = 41
    Caption = 'Write'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 3
    OnClick = cmdWriteClick
  end
  object cmdExit: TButton
    Left = 504
    Top = 536
    Width = 113
    Height = 41
    Caption = 'Exit'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 4
    OnClick = cmdExitClick
  end
  object lblMessage: TStaticText
    Left = 8
    Top = 16
    Width = 617
    Height = 26
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
    TabOrder = 5
  end
  object txtDepartment: TEdit
    Left = 144
    Top = 64
    Width = 321
    Height = 27
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ImeName = 'Korean Input System (IME 2000)'
    ParentFont = False
    TabOrder = 6
  end
end
