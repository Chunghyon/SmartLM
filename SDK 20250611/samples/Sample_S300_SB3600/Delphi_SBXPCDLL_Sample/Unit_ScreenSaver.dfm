object frmScreenSaver: TfrmScreenSaver
  Left = 352
  Top = 152
  Width = 542
  Height = 695
  Caption = 'frmScreenSaver'
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  OnClose = FormClose
  PixelsPerInch = 96
  TextHeight = 13
  object Label3: TLabel
    Left = 48
    Top = 72
    Width = 103
    Height = 19
    Caption = 'Customer Name:'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
  end
  object Label1: TLabel
    Left = 48
    Top = 120
    Width = 103
    Height = 19
    Caption = 'Company Name:'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
  end
  object Label5: TLabel
    Left = 32
    Top = 256
    Width = 95
    Height = 19
    Caption = 'Sleep Message:'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
  end
  object Label2: TLabel
    Left = 32
    Top = 320
    Width = 80
    Height = 19
    Caption = 'Glyph Width:'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
  end
  object Label4: TLabel
    Left = 32
    Top = 360
    Width = 79
    Height = 19
    Caption = 'Glyph Height'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
  end
  object Label6: TLabel
    Left = 32
    Top = 400
    Width = 74
    Height = 19
    Caption = 'Font Height:'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
  end
  object Label7: TLabel
    Left = 32
    Top = 440
    Width = 72
    Height = 19
    Caption = 'Font Width:'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
  end
  object Label8: TLabel
    Left = 32
    Top = 480
    Width = 78
    Height = 19
    Caption = 'Font Weight:'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
  end
  object lblMessage: TStaticText
    Left = 8
    Top = 16
    Width = 513
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
    TabOrder = 0
  end
  object txtCustomerName: TEdit
    Left = 192
    Top = 64
    Width = 289
    Height = 27
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ImeName = 'Korean Input System (IME 2000)'
    ParentFont = False
    TabOrder = 1
  end
  object txtCompanyName: TEdit
    Left = 192
    Top = 112
    Width = 289
    Height = 27
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ImeName = 'Korean Input System (IME 2000)'
    ParentFont = False
    TabOrder = 2
  end
  object cmdGetCustomerInfo: TButton
    Left = 61
    Top = 176
    Width = 180
    Height = 41
    Caption = 'Get Customer Info'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 3
    OnClick = cmdGetCustomerInfoClick
  end
  object cmdSetCustomerInfo: TButton
    Left = 285
    Top = 176
    Width = 164
    Height = 41
    Caption = 'Set Customer Info'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 4
    OnClick = cmdSetCustomerInfoClick
  end
  object txtGlyphHeight: TEdit
    Left = 144
    Top = 352
    Width = 113
    Height = 27
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ImeName = 'Korean Input System (IME 2000)'
    ParentFont = False
    ReadOnly = True
    TabOrder = 5
  end
  object txtGlyphWidth: TEdit
    Left = 144
    Top = 312
    Width = 113
    Height = 27
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ImeName = 'Korean Input System (IME 2000)'
    ParentFont = False
    ReadOnly = True
    TabOrder = 6
  end
  object txtFontWidth: TEdit
    Left = 144
    Top = 432
    Width = 113
    Height = 27
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ImeName = 'Korean Input System (IME 2000)'
    ParentFont = False
    TabOrder = 7
    Text = '11'
  end
  object txtFontHeight: TEdit
    Left = 144
    Top = 392
    Width = 113
    Height = 27
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ImeName = 'Korean Input System (IME 2000)'
    ParentFont = False
    TabOrder = 8
    Text = '20'
  end
  object txtFontWeight: TEdit
    Left = 144
    Top = 472
    Width = 113
    Height = 27
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ImeName = 'Korean Input System (IME 2000)'
    ParentFont = False
    TabOrder = 9
    Text = '700'
  end
  object cmdGetSleepMessage: TButton
    Left = 13
    Top = 592
    Width = 180
    Height = 41
    Caption = 'Get Sleep Message'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 10
    OnClick = cmdGetSleepMessageClick
  end
  object cmdSetSleepMessage: TButton
    Left = 208
    Top = 592
    Width = 177
    Height = 41
    Caption = 'Set Sleep Message'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 11
    OnClick = cmdSetSleepMessageClick
  end
  object cmdExit: TButton
    Left = 400
    Top = 592
    Width = 105
    Height = 41
    Caption = 'Exit'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 12
    OnClick = cmdExitClick
  end
  object chkItalic: TCheckBox
    Left = 320
    Top = 400
    Width = 121
    Height = 33
    Caption = 'Italic'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
    TabOrder = 13
  end
  object chkUnderline: TCheckBox
    Left = 320
    Top = 432
    Width = 121
    Height = 33
    Caption = 'Underline'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
    TabOrder = 14
  end
  object chkStrikeOut: TCheckBox
    Left = 320
    Top = 464
    Width = 121
    Height = 33
    Caption = 'StrikeOut'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
    TabOrder = 15
  end
  object chkDebugOut: TCheckBox
    Left = 32
    Top = 536
    Width = 121
    Height = 33
    Caption = 'DebugOut'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
    TabOrder = 16
  end
  object txtDebugOutFile: TEdit
    Left = 160
    Top = 536
    Width = 289
    Height = 27
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ImeName = 'Korean Input System (IME 2000)'
    ParentFont = False
    TabOrder = 17
    Text = 'C:\TempPhoto.bmp'
  end
  object cmdGetGlyphSize: TButton
    Left = 293
    Top = 328
    Width = 180
    Height = 41
    Caption = 'Get Glyph Size'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 18
    OnClick = cmdGetGlyphSizeClick
  end
  object cmdDebugOutFileBrowse: TButton
    Left = 448
    Top = 536
    Width = 41
    Height = 33
    Caption = '...'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 19
    OnClick = cmdDebugOutFileBrowseClick
  end
  object txtSleepMessage: TEdit
    Left = 168
    Top = 256
    Width = 337
    Height = 27
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ImeName = 'Korean Input System (IME 2000)'
    ParentFont = False
    TabOrder = 20
  end
end
