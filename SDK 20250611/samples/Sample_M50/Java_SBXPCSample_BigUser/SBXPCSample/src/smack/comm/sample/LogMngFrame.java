package smack.comm.sample;

import java.awt.Color;
import java.awt.Cursor;
import java.awt.EventQueue;
import java.awt.Font;
import java.awt.event.WindowAdapter;
import java.awt.event.WindowEvent;
import java.text.DecimalFormat;
import java.text.NumberFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.List;

import javax.swing.ImageIcon;
import javax.swing.JButton;
import javax.swing.JCheckBox;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JOptionPane;
import javax.swing.JPanel;
import javax.swing.JRadioButton;
import javax.swing.JScrollPane;
import javax.swing.JTable;
import javax.swing.JTextField;
import javax.swing.SwingConstants;
import javax.swing.UIManager;
import javax.swing.border.BevelBorder;
import javax.swing.border.TitledBorder;
import javax.swing.table.AbstractTableModel;

import smack.comm.SBXPCProxy;
import smack.comm.data_biguser.GeneralLogData;
import smack.comm.data_biguser.SuperLogData;
import smack.comm.output.GeneralLogDataOutput;
import smack.comm.output.OneStringOutput;
import smack.comm.output.SuperLogDataOutput;
import smack.comm.output.XMLParseBinaryByteOutput;
import smack.comm.sample.global.SysUtil;
import java.awt.event.ActionListener;
import java.awt.event.ActionEvent;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import java.awt.GridLayout;

public class LogMngFrame extends JFrame {
	private JLabel lblMessage;
	private JLabel lblLogData;
	private JLabel lblTotal;
	private JTable tblLogList;
	private JScrollPane jScrollPane1;
	private JCheckBox chkSearchAndDelete;
	private JCheckBox chkReadMark;
	private JCheckBox chkReadGlogExt;
	private JCheckBox chkShowGLogPhoto;
	private JLabel lblGLogPhoto;
	private JButton btnDeleteGLogPhotoOne;
	private JButton btnClearPhotoGlog;
	private JButton btnClearPhotoUser;
	private JButton btnClearPhotoAll;
	private JButton btnReadSLogData;
	private JButton btnReadAllSLogData;
	private JButton btnEmptySLogData;
	private JButton btnReadGLogData;
	private JButton btnReadAllGLogData;
	private JButton btnEmptyGLogData;

	private JCheckBox chkSLog;
	private JCheckBox chkGLog;
	private JLabel lblStartPos;
	private JLabel lblLogCount;
	private JLabel lblArrow;
	private JTextField txtStartPos;
	private JTextField txtLogCount;
	private JButton btnGetLogPosInfo;
	private JButton btnReadLogWithPos;
	private JButton btnDeleteLogWithPos;
	
	SLogDataListModel superLogListModel = new SLogDataListModel();
	GLogDataListModel generalLogListModel = new GLogDataListModel();

	private int prevSelectedRow = -1;
	private JPanel panel_2;
	private JPanel panel_3;
	
	private final int PhotoType_UserPhoto = 1;
	private final int PhotoType_GlogPhoto = 2;
	private final int PhotoType_All = -1;

	/**
	 * Launch the application.
	 */
	public static void main(String[] args) {
		EventQueue.invokeLater(new Runnable() {
			public void run() {
				try {
					LogMngFrame frame = new LogMngFrame();
					frame.setVisible(true);
				} catch (Exception e) {
					e.printStackTrace();
				}
			}
		});
	}

	/**
	 * Create the frame.
	 */
	public LogMngFrame() {
		addWindowListener(new WindowAdapter() {
			@Override
			public void windowClosing(WindowEvent arg0) {
				if (MainFrame.getInstance() != null)
					MainFrame.getInstance().setVisible(true);
			}
		});

		setTitle("Log Management");
		setBounds(100, 100, 836, 669);
		setDefaultCloseOperation(javax.swing.WindowConstants.DISPOSE_ON_CLOSE);
		getContentPane().setLayout(null);

		lblMessage = new JLabel("Message");
		lblMessage.setHorizontalAlignment(SwingConstants.CENTER);
		lblMessage.setFont(new Font("Segoe UI", Font.BOLD, 18));
		lblMessage.setBorder(javax.swing.BorderFactory.createBevelBorder(javax.swing.border.BevelBorder.LOWERED));
		lblMessage.setBounds(10, 11, 637, 32);
		getContentPane().add(lblMessage);

		lblLogData = new JLabel("Log Data ");
		lblLogData.setBounds(10, 54, 70, 14);
		getContentPane().add(lblLogData);

		lblTotal = new JLabel("Total :");
		lblTotal.setBounds(145, 54, 157, 14);
		getContentPane().add(lblTotal);

		jScrollPane1 = new JScrollPane();
		jScrollPane1.setBounds(10, 79, 637, 459);
		getContentPane().add(jScrollPane1);

		tblLogList = new JTable();
		tblLogList.addMouseListener(new MouseAdapter() {
			@Override
			public void mouseClicked(MouseEvent arg0) {
				tblLogListMouseClicked(arg0);
			}
		});
		jScrollPane1.setViewportView(tblLogList);

		chkSearchAndDelete = new JCheckBox("and Delete");
		chkSearchAndDelete.setBounds(435, 50, 97, 23);
		getContentPane().add(chkSearchAndDelete);

		chkReadMark = new JCheckBox("ReadMark");
		chkReadMark.setSelected(true);
		chkReadMark.setBounds(534, 50, 97, 23);
		getContentPane().add(chkReadMark);
		
		chkReadGlogExt = new JCheckBox("Read GlogExt(BodyTemp,Mood)");
		chkReadGlogExt.setBounds(632, 50, 179, 23);
		getContentPane().add(chkReadGlogExt);

		chkShowGLogPhoto = new JCheckBox("Show GLog Photo");
		chkShowGLogPhoto.setSelected(true);
		chkShowGLogPhoto.setBounds(668, 79, 125, 23);
		getContentPane().add(chkShowGLogPhoto);

		lblGLogPhoto = new JLabel("");
		lblGLogPhoto.setBorder(new BevelBorder(BevelBorder.LOWERED, null, null, null, null));
		lblGLogPhoto.setBounds(657, 109, 139, 143);
		getContentPane().add(lblGLogPhoto);

		btnDeleteGLogPhotoOne = new JButton("DeleteGLogPhoto(one)");
		btnDeleteGLogPhotoOne.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				btnDeleteGLogPhotoOne_actionPerformed(arg0);
			}
		});
		btnDeleteGLogPhotoOne.setBounds(657, 263, 154, 34);
		getContentPane().add(btnDeleteGLogPhotoOne);

		btnClearPhotoGlog = new JButton("Clear All Glog Photo");
		btnClearPhotoGlog.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				btnClearPhoto_actionPerformed(arg0, PhotoType_GlogPhoto);
			}
		});
		btnClearPhotoGlog.setBounds(657, 298, 154, 29);
		getContentPane().add(btnClearPhotoGlog);

		btnClearPhotoUser = new JButton("Clear All Enroll Photo");
		btnClearPhotoUser.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				btnClearPhoto_actionPerformed(arg0, PhotoType_UserPhoto);
			}
		});
		btnClearPhotoUser.setBounds(657, 328, 154, 29);
		getContentPane().add(btnClearPhotoUser);

		btnClearPhotoAll = new JButton("Clear All Photo");
		btnClearPhotoAll.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				btnClearPhoto_actionPerformed(arg0, PhotoType_All);
			}
		});
		btnClearPhotoAll.setBounds(657, 358, 154, 29);
		getContentPane().add(btnClearPhotoAll);

	
		panel_2 = new JPanel();
		panel_2.setBorder(
				new TitledBorder(UIManager.getBorder("TitledBorder.border"), "", TitledBorder.LEADING, TitledBorder.TOP, null, new Color(0, 0, 0)));
		panel_2.setBounds(10, 549, 637, 70);
		getContentPane().add(panel_2);
		panel_2.setLayout(new GridLayout(0, 6, 0, 0));

		btnReadSLogData = new JButton("<html>Read<br>SLog Data</html>");
		panel_2.add(btnReadSLogData);

		btnReadAllSLogData = new JButton("<html>Read All<br>SLog Data</html>");
		panel_2.add(btnReadAllSLogData);

		btnEmptySLogData = new JButton("<html>Empty<br>SLog Data</html>");
		panel_2.add(btnEmptySLogData);

		btnReadGLogData = new JButton("<html>Read<br>GLog Data</html>");
		panel_2.add(btnReadGLogData);

		btnReadAllGLogData = new JButton("<html>Read All<br>GLog Data</html>");
		panel_2.add(btnReadAllGLogData);

		btnEmptyGLogData = new JButton("<html>Empty<br>GLog Data</html>");
		panel_2.add(btnEmptyGLogData);
		btnEmptyGLogData.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				btnEmptyGLogData_actionPerformed(arg0);
			}
		});
		btnReadAllGLogData.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				btnReadAllGLogData_actionPerformed(arg0);
			}
		});
		btnReadGLogData.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				btnReadGLogData_actionPerformed(arg0);
			}
		});
		btnEmptySLogData.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				btnEmptySLogData_actionPerformed(arg0);
			}
		});
		btnReadAllSLogData.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				btnReadAllSLogData_actionPerformed(arg0);
			}
		});
		btnReadSLogData.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				btnReadSLogData_actionPerformed(arg0);
			}
		});
		
		panel_3 = new JPanel();
		panel_3.setBorder(
				new TitledBorder(UIManager.getBorder("TitledBorder.border"), "Log By Position", TitledBorder.LEADING, TitledBorder.TOP, null, new Color(0, 0, 0)));
		panel_3.setBounds(650, 400, 165, 221);
		getContentPane().add(panel_3);
		panel_3.setLayout(null);
		
		chkSLog = new JCheckBox("SLog");
		chkSLog.setSelected(true);
		chkSLog.setBounds(10, 20, 70, 29);
		chkSLog.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				if (!chkSLog.isSelected())
					chkSLog.setSelected(true);
				else
				{
					chkGLog.setSelected(false);
					txtStartPos.setText("");
					txtLogCount.setText("");
				}
			}
		});
		panel_3.add(chkSLog);

		chkGLog = new JCheckBox("GLog");
		chkGLog.setBounds(90, 20, 70, 29);
		chkGLog.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				if (!chkGLog.isSelected())
					chkGLog.setSelected(true);
				else
				{
					chkSLog.setSelected(false);
					txtStartPos.setText("");
					txtLogCount.setText("");
				}
			}
		});
		panel_3.add(chkGLog);
		
		lblStartPos = new JLabel("StartPos:");
		lblStartPos.setBounds(10, 50, 70, 29);
		panel_3.add(lblStartPos);
		lblLogCount = new JLabel("Count:");
		lblLogCount.setBounds(90, 50, 70, 29);
		panel_3.add(lblLogCount);
		txtStartPos = new JTextField();
		txtStartPos.setBounds(10, 80, 60, 29);
		panel_3.add(txtStartPos);
		lblArrow = new JLabel("-->");
		lblArrow.setBounds(75, 80, 15, 29);
		panel_3.add(lblArrow);
		txtLogCount = new JTextField();
		txtLogCount.setBounds(97, 80, 60, 29);
		panel_3.add(txtLogCount);
		btnGetLogPosInfo = new JButton("Get LogPos Info");
		btnGetLogPosInfo.setBounds(10, 110, 146, 29);
		btnGetLogPosInfo.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				btnGetLogPosInfo_actionPerformed(arg0, chkSLog.isSelected());
			}
		});
		panel_3.add(btnGetLogPosInfo);
		btnReadLogWithPos = new JButton("Read Log with Pos");
		btnReadLogWithPos.setBounds(10, 140, 146, 29);
		btnReadLogWithPos.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				btnReadLogWithPos_actionPerformed(arg0, chkSLog.isSelected());
			}
		});
		panel_3.add(btnReadLogWithPos);
		btnDeleteLogWithPos = new JButton("Delete Log with Pos");
		btnDeleteLogWithPos.setBounds(10, 170, 146, 29);
		btnDeleteLogWithPos.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				btnDeleteLogWithPos_actionPerformed(arg0, chkSLog.isSelected());
			}
		});
		panel_3.add(btnDeleteLogWithPos);
	}

	private void btnReadSLogData_actionPerformed(ActionEvent evt) {
		superLogListModel.removeAll();
		tblLogList.setModel(superLogListModel);

		boolean ret;
		int errorCode;

		lblMessage.setText(SysUtil.WORKING);
		invalidate();

		ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, false);

		if (!ret) {
			lblMessage.setText(SysUtil.NO_DEVICE);
			return;
		}

		ret = SBXPCProxy.ReadSuperLogData(SysUtil.MachineNumber, chkReadMark.isSelected());

		if (ret) {
			errorCode = 0;

			if (chkSearchAndDelete.isSelected())
				ret = SBXPCProxy.EmptySuperLogData(SysUtil.MachineNumber);

			lblMessage.setText("Getting...");
			invalidate();

			SuperLogDataOutput output;
			while (true) {
				output = SBXPCProxy.GetSuperLogData(SysUtil.MachineNumber);

				if (!output.isSuccess())
					break;

				OneStringOutput outputId;
				outputId = SBXPCProxy.GetLastBigUserId_AsString(SysUtil.MachineNumber, true);
				if (!outputId.isSuccess())
					break;
				
				superLogListModel.addSLogData(new SuperLogData(output, outputId.value));
			}
			lblTotal.setText("Total: " + superLogListModel.getRowCount());
		} else {
			errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;
		}

		lblMessage.setText(SysUtil.ErrorPrint(errorCode));

		ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, true);

	}

	private void btnReadAllSLogData_actionPerformed(ActionEvent evt) {
		superLogListModel.removeAll();
		tblLogList.setModel(superLogListModel);

		boolean ret;
		int errorCode;

		lblMessage.setText(SysUtil.WORKING);
		invalidate();

		ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, false);

		if (!ret) {
			lblMessage.setText(SysUtil.NO_DEVICE);
			return;
		}

		ret = SBXPCProxy.ReadAllSLogData(SysUtil.MachineNumber);

		if (ret) {
			errorCode = 0;

			if (chkSearchAndDelete.isSelected())
				ret = SBXPCProxy.EmptySuperLogData(SysUtil.MachineNumber);

			lblMessage.setText("Getting...");
			invalidate();

			SuperLogDataOutput output;
			while (true) {
				output = SBXPCProxy.GetAllSLogData(SysUtil.MachineNumber);

				if (!output.isSuccess())
					break;

				OneStringOutput outputId;
				outputId = SBXPCProxy.GetLastBigUserId_AsString(SysUtil.MachineNumber, true);
				if (!outputId.isSuccess())
					break;
				
				superLogListModel.addSLogData(new SuperLogData(output, outputId.value));
			}
			lblTotal.setText("Total: " + superLogListModel.getRowCount());
		} else {
			errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;
		}

		lblMessage.setText(SysUtil.ErrorPrint(errorCode));

		ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, true);
	}

	private void btnEmptySLogData_actionPerformed(ActionEvent evt) {
		superLogListModel.removeAll();
		tblLogList.setModel(superLogListModel);

		boolean ret;
		int errorCode;

		lblMessage.setText(SysUtil.WORKING);
		invalidate();

		ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, false);

		if (!ret) {
			lblMessage.setText(SysUtil.NO_DEVICE);
			return;
		}

		ret = SBXPCProxy.EmptySuperLogData(SysUtil.MachineNumber);

		if (ret)
			errorCode = 0;
		else
			errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;

		lblMessage.setText(SysUtil.ErrorPrint(errorCode));

		ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, true);
	}

	private void btnEmptyGLogData_actionPerformed(ActionEvent evt) {
		superLogListModel.removeAll();
		tblLogList.setModel(superLogListModel);

		boolean ret;
		int errorCode;

		lblMessage.setText(SysUtil.WORKING);
		invalidate();

		ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, false);

		if (!ret) {
			lblMessage.setText(SysUtil.NO_DEVICE);
			return;
		}

		ret = SBXPCProxy.EmptyGeneralLogData(SysUtil.MachineNumber);

		if (ret)
			errorCode = 0;
		else
			errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;

		lblMessage.setText(SysUtil.ErrorPrint(errorCode));

		ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, true);
	}

	public static void lockJTableColumnWidth(javax.swing.table.TableColumn tc, int pixels) {
       if (tc != null) {
           tc.setMinWidth(pixels);
           tc.setMaxWidth(pixels);
           tc.setPreferredWidth(pixels);
           tc.setResizable(pixels > 0 ? true : false);
       }
   }
	private void prepareGLog() {
		generalLogListModel.removeAll();
		tblLogList.setModel(generalLogListModel);

		lockJTableColumnWidth(tblLogList.getColumnModel().getColumn(2), chkReadGlogExt.isSelected() ? 80 : 0);
		lockJTableColumnWidth(tblLogList.getColumnModel().getColumn(3), chkReadGlogExt.isSelected() ? 80 : 0);
	}
	
	private void btnReadGLogData_actionPerformed(ActionEvent evt) {
		prepareGLog();

		boolean ret;
		int errorCode;

		lblMessage.setText(SysUtil.WORKING);
		invalidate();

		ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, false);

		if (!ret) {
			lblMessage.setText(SysUtil.NO_DEVICE);
			return;
		}

		if (chkReadGlogExt.isSelected())
			ret = SBXPCProxy.ReadGeneralLogData_Ext(SysUtil.MachineNumber, chkReadMark.isSelected());
		else
			ret = SBXPCProxy.ReadGeneralLogData(SysUtil.MachineNumber, chkReadMark.isSelected());

		if (ret) {
			errorCode = 0;

			if (chkSearchAndDelete.isSelected())
				ret = SBXPCProxy.EmptyGeneralLogData(SysUtil.MachineNumber);

			lblMessage.setText("Getting...");
			invalidate();

			GeneralLogDataOutput output;
			while (true) {
				if (chkReadGlogExt.isSelected())
					output = SBXPCProxy.GetGeneralLogData_Ext(SysUtil.MachineNumber);
				else
					output = SBXPCProxy.GetGeneralLogData(SysUtil.MachineNumber);

				if (!output.isSuccess())
					break;

				OneStringOutput outputId;
				outputId = SBXPCProxy.GetLastBigUserId_AsString(SysUtil.MachineNumber, true);
				if (!outputId.isSuccess())
					break;
				
				generalLogListModel.addGLogData(new GeneralLogData(output, outputId.value));
			}
			lblTotal.setText("Total: " + generalLogListModel.getRowCount());
		} else {
			errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;
		}

		lblMessage.setText(SysUtil.ErrorPrint(errorCode));

		ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, true);
	}

	private void btnReadAllGLogData_actionPerformed(ActionEvent evt) {
		prepareGLog();

		boolean ret;
		int errorCode;

		lblMessage.setText(SysUtil.WORKING);
		invalidate();

		ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, false);

		if (!ret) {
			lblMessage.setText(SysUtil.NO_DEVICE);
			return;
		}

		if (chkReadGlogExt.isSelected())
			ret = SBXPCProxy.ReadAllGLogData_Ext(SysUtil.MachineNumber);
		else
			ret = SBXPCProxy.ReadAllGLogData(SysUtil.MachineNumber);

		if (ret) {
			errorCode = 0;

			if (chkSearchAndDelete.isSelected())
				ret = SBXPCProxy.EmptyGeneralLogData(SysUtil.MachineNumber);

			lblMessage.setText("Getting...");
			invalidate();

			GeneralLogDataOutput output;
			while (true) {
				if (chkReadGlogExt.isSelected())
					output = SBXPCProxy.GetAllGLogData_Ext(SysUtil.MachineNumber);
				else
					output = SBXPCProxy.GetAllGLogData(SysUtil.MachineNumber);

				if (!output.isSuccess())
					break;

				OneStringOutput outputId;
				outputId = SBXPCProxy.GetLastBigUserId_AsString(SysUtil.MachineNumber, true);
				if (!outputId.isSuccess())
					break;
				
				generalLogListModel.addGLogData(new GeneralLogData(output, outputId.value));
			}
			lblTotal.setText("Total: " + generalLogListModel.getRowCount());
		} else {
			errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;
		}

		lblMessage.setText(SysUtil.ErrorPrint(errorCode));

		ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, true);
	}

	private void tblLogListMouseClicked(java.awt.event.MouseEvent evt) {
		if (!chkShowGLogPhoto.isSelected())
			return;
		int selectedRow = tblLogList.getSelectedRow();
		if (!(tblLogList.getModel() instanceof GLogDataListModel))
			return;
		if (selectedRow == prevSelectedRow)
			return;
		prevSelectedRow = selectedRow;

		lblGLogPhoto.setIcon(null);

		if ("No Photo".equals(tblLogList.getValueAt(selectedRow, 0)))
			return;

		boolean ret;
		int errorCode;

		lblMessage.setText(SysUtil.WORKING);
		invalidate();

		ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, false);

		if (!ret) {
			lblMessage.setText(SysUtil.NO_DEVICE);
			return;
		}

		long photoPos = Long.parseLong(tblLogList.getValueAt(selectedRow, 0).toString());
		OneStringOutput output;
		output = SysUtil.MakeXMLCommandHeader("GetGLogPhotoData");
		output = SBXPCProxy.XML_AddLong(output.value, "PhotoPos", photoPos);

		output = SBXPCProxy.GeneralOperationXML(SysUtil.MachineNumber, output.value);

		if (output.isSuccess()) {
			errorCode = 0;
			XMLParseBinaryByteOutput parseOutput;
			int photo_size = SBXPCProxy.XML_ParseInt(output.value, "PhotoSize");
			if (photo_size <= 0)
				photo_size = SysUtil.COMPRESSED_PHOTO_SIZE_8K;
			parseOutput = SBXPCProxy.XML_ParseBinaryByte(output.value, "PhotoData", photo_size);
			if (parseOutput.isSuccess())
				lblGLogPhoto.setIcon(new ImageIcon(parseOutput.pData, "TEMP_GLOG_PHOTO"));
		} else {
			errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;
		}

		lblMessage.setText(SysUtil.ErrorPrint(errorCode));

		ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, true);
	}

	private void btnDeleteGLogPhotoOne_actionPerformed(ActionEvent evt) {
		int selectedRow = tblLogList.getSelectedRow();
		if (selectedRow == -1) {
			lblMessage.setText("Please select log to delete photo.");
			return;
		}
		if ("No Photo".equals(tblLogList.getValueAt(selectedRow, 0))) {
			lblMessage.setText("No Photo Id");
			return;
		}
		long photoPos = Long.parseLong(tblLogList.getValueAt(selectedRow, 0).toString());
		OneStringOutput output;
		output = SysUtil.MakeXMLCommandHeader("DeleteGLogPhotoData");
		output = SBXPCProxy.XML_AddLong(output.value, "PhotoPos", photoPos);

		output = SBXPCProxy.GeneralOperationXML(SysUtil.MachineNumber, output.value);

		if (output.isSuccess()) {
			String strResultCode = SBXPCProxy.XML_ParseString(output.value, "ResultCode").value;
			if (strResultCode.equals("Success"))
				lblMessage.setText("Delete GLog Photo Success. (photoPos=" + String.valueOf(photoPos) + ")");
			else if (strResultCode.equals("No Photo"))
				lblMessage.setText("No Photo");
			else if (strResultCode.equals("InvalidParam"))
				lblMessage.setText("Invalid Parameter.");
			else
				lblMessage.setText("Unknown Error");
		} else {
			int errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;
			lblMessage.setText(SysUtil.ErrorPrint(errorCode));
		}
	}

	private void btnClearPhoto_actionPerformed(ActionEvent evt, int PhotoType)
	{
		OneStringOutput output;
		output = SysUtil.MakeXMLCommandHeader("ClearPhoto");
		output = SBXPCProxy.XML_AddInt(output.value, "PhotoType", PhotoType);

		output = SBXPCProxy.GeneralOperationXML(SysUtil.MachineNumber, output.value);

		if (output.isSuccess()) {
			String strResultCode = SBXPCProxy.XML_ParseString(output.value, "ResultCode").value;
			if (strResultCode.equals("Success"))
			{
				switch (PhotoType)
				{
				case PhotoType_GlogPhoto: lblMessage.setText("Clear All GLog Photo Success."); break;
				case PhotoType_UserPhoto: lblMessage.setText("Clear All Enroll Photo Success."); break;
				default:
				case PhotoType_All: lblMessage.setText("Clear All Photo Success."); break;
				}
			}
			else
				lblMessage.setText("Unknown Error");
		} else {
			int errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;
			lblMessage.setText(SysUtil.ErrorPrint(errorCode));
		}
	}
	private void btnGetLogPosInfo_actionPerformed(ActionEvent evt, boolean isSLog)
	{
		OneStringOutput output;
		if (isSLog)
			output = SysUtil.MakeXMLCommandHeader("GetSLogPosInfo");
		else
			output = SysUtil.MakeXMLCommandHeader("GetGLogPosInfo");
		
		output = SBXPCProxy.GeneralOperationXML(SysUtil.MachineNumber, output.value);

		if (output.isSuccess()) {
			int LogCount = SBXPCProxy.XML_ParseInt(output.value, "LogCount");
			int StartPos = SBXPCProxy.XML_ParseInt(output.value, "StartPos");
			int MaxCount = SBXPCProxy.XML_ParseInt(output.value, "MaxCount");
			
			txtStartPos.setText(Integer.toString(StartPos));
			txtLogCount.setText(Integer.toString(LogCount));
			lblMessage.setText(
					(isSLog ? "[SLog] " : "[GLog] ") + 
					"LogCount=" + Integer.toString(LogCount)+
					", MaxCount=" + Integer.toString(MaxCount) +
					", ValidPos: " + Integer.toString(StartPos) + "~" + Integer.toString((StartPos + LogCount) % (MaxCount + 1)));
		} else {
			int errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;
			lblMessage.setText(SysUtil.ErrorPrint(errorCode));
		}
	}
	private void btnReadLogWithPos_actionPerformed(ActionEvent evt, boolean isSLog)
	{
		int start = -1, count = -1;
		try {
			start = Integer.parseInt(txtStartPos.getText());
			count = Integer.parseInt(txtLogCount.getText());
		} catch (NumberFormatException ne) {
		}	
		
		if (start < 0)
		{
			JOptionPane.showMessageDialog(null, "StartPos must be equal to or greater than zero!");
			txtStartPos.requestFocus();
			txtStartPos.selectAll();
			return;
		}
		if (count <= 0)
		{
			JOptionPane.showMessageDialog(null, "Count must be greater than zero!");
			txtLogCount.requestFocus();
			txtLogCount.selectAll();
			return;
		}
		
		if (isSLog)
		{
			superLogListModel.removeAll();
			tblLogList.setModel(superLogListModel);
		}
		else
		{
			prepareGLog();
		}

		boolean ret;
		int errorCode;

		lblMessage.setText(SysUtil.WORKING);
		lblTotal.setText("Total: ");
		invalidate();

		ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, false);

		if (!ret) {
			lblMessage.setText(SysUtil.NO_DEVICE);
			return;
		}

		if (isSLog)
		{
			ret = SBXPCProxy.ReadSLogWithPos(SysUtil.MachineNumber, start, count);
		}
		else
		{
			if (chkReadGlogExt.isSelected())
				ret = SBXPCProxy.ReadGLogWithPos_Ext(SysUtil.MachineNumber, start, count);
			else
				ret = SBXPCProxy.ReadGLogWithPos(SysUtil.MachineNumber, start, count);
		}
			
		if (ret) {
			errorCode = 0;

			lblMessage.setText("Getting...");
			invalidate();

			if (isSLog)
			{
				SuperLogDataOutput output;
				while (true) {
					output = SBXPCProxy.GetAllSLogData(SysUtil.MachineNumber);
	
					if (!output.isSuccess())
						break;
	
					OneStringOutput outputId;
					outputId = SBXPCProxy.GetLastBigUserId_AsString(SysUtil.MachineNumber, true);
					if (!outputId.isSuccess())
						break;
					
					superLogListModel.addSLogData(new SuperLogData(output, outputId.value));
				}
				lblTotal.setText("Total: " + superLogListModel.getRowCount());
			}
			else
			{
				GeneralLogDataOutput output;
				while (true) {
					if (chkReadGlogExt.isSelected())
						output = SBXPCProxy.GetAllGLogData_Ext(SysUtil.MachineNumber);
					else
						output = SBXPCProxy.GetAllGLogData(SysUtil.MachineNumber);
	
					if (!output.isSuccess())
						break;
	
					OneStringOutput outputId;
					outputId = SBXPCProxy.GetLastBigUserId_AsString(SysUtil.MachineNumber, true);
					if (!outputId.isSuccess())
						break;
					
					generalLogListModel.addGLogData(new GeneralLogData(output, outputId.value));
				}
				lblTotal.setText("Total: " + generalLogListModel.getRowCount());
			}
		} else {
			errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;
		}

		lblMessage.setText("ReadLogWithPos " + SysUtil.ErrorPrint(errorCode));

		ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, true);
	}

	private void btnDeleteLogWithPos_actionPerformed(ActionEvent evt, boolean isSLog)
	{
		int count = -1;
		try {
			count = Integer.parseInt(txtLogCount.getText());
		} catch (NumberFormatException ne) {
		}	
		
		if (count < 0)
		{
			JOptionPane.showMessageDialog(null, "Count must be equal to or greater than zero!");
			txtLogCount.requestFocus();
			txtLogCount.selectAll();
			return;
		}
		
		txtStartPos.setText("");
		
		OneStringOutput output;
		if (isSLog)
			output = SysUtil.MakeXMLCommandHeader("DeleteSLogWithPos");
		else
			output = SysUtil.MakeXMLCommandHeader("DeleteGLogWithPos");
		output = SBXPCProxy.XML_AddInt(output.value, "EndPos", count);

		output = SBXPCProxy.GeneralOperationXML(SysUtil.MachineNumber, output.value);

		if (output.isSuccess()) {
			lblMessage.setText(
					(isSLog ? "[SLog] " : "[GLog] ") + 
					"DeleteLogWithPos" + "Success");
		} else {
			int errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;
			lblMessage.setText(SysUtil.ErrorPrint(errorCode));
		}
	}
	
	///////////////////////////////////
	class SLogDataListModel extends AbstractTableModel {
		final String[] COLUMN_TEXT = { "TMNo", "strSEnlNo", "SMNo", "GEnlNo", "GMNo", "Manipulation", "FpNo",
				"DateTime", };
		List<SuperLogData> logList;

		public void addSLogData(SuperLogData logData) {
			logList.add(logData);
			fireTableRowsInserted(logList.size() - 1, logList.size() - 1);
		}

		public void removeAll() {
			logList.removeAll(logList);
			fireTableDataChanged();
		}

		public SLogDataListModel() {
			logList = new ArrayList<SuperLogData>();
		}

		public int getRowCount() {
			return logList.size();
		}

		public int getColumnCount() {
			return COLUMN_TEXT.length;
		}

		public Object getValueAt(int rowIndex, int columnIndex) {
			SuperLogData logData = logList.get(rowIndex);
			switch (columnIndex) {
			case 0:
				return logData.dwTMachineNumber;
			case 1:
				return logData.strSEnrollNumber;
			case 2:
				return logData.dwSMachineNumber;
			case 3:
				return logData.dwGEnrollNumber;
			case 4:
				return logData.dwGMachineNumber;
			case 5: {
				String ret;
				switch ((int) logData.dwManipulation) {
				case 3:
					ret = String.valueOf(logData.dwManipulation) + "--" + "Enroll User";
					break;
				case 4:
					ret = String.valueOf(logData.dwManipulation) + "--" + "Enroll Manager";
					break;
				case 5:
					ret = String.valueOf(logData.dwManipulation) + "--" + "Delete Fp Data";
					break;
				case 6:
					ret = String.valueOf(logData.dwManipulation) + "--" + "Delete Password";
					break;
				case 7:
					ret = String.valueOf(logData.dwManipulation) + "--" + "Delete Card Data";
					break;
				case 8:
					ret = String.valueOf(logData.dwManipulation) + "--" + "Delete All LogData";
					break;
				case 9:
					ret = String.valueOf(logData.dwManipulation) + "--" + "Modify System Info";
					break;
				case 10:
					ret = String.valueOf(logData.dwManipulation) + "--" + "Modify System Time";
					break;
				case 11:
					ret = String.valueOf(logData.dwManipulation) + "--" + "Modify Log Setting";
					break;
				case 12:
					ret = String.valueOf(logData.dwManipulation) + "--" + "Modify Comm Setting";
					break;
				case 13:
					ret = String.valueOf(logData.dwManipulation) + "--" + "Modify Timezone Setting";
					break;
				case 14:
					ret = String.valueOf(logData.dwManipulation) + "--" + "Delete Face";
					break;

				case 15:
					ret = String.valueOf(logData.dwManipulation) + "--" + "Reset All Settings";
					break;
				case 16:
					ret = String.valueOf(logData.dwManipulation) + "--" + "Delete All Log";
					break;
				case 19:
					ret = String.valueOf(logData.dwManipulation) + "--" + "Modify UserTimezone";
					break;
				case 20:
					ret = String.valueOf(logData.dwManipulation) + "--" + "Turn On";
				case 24:
					ret = String.valueOf(logData.dwManipulation) + "--" + "DoorOpenTimeout Alarm";
					break;
				case 25:
					ret = String.valueOf(logData.dwManipulation) + "--" + "Illegal DoorOpen Alarm";
					break;
				case 26:
					ret = String.valueOf(logData.dwManipulation) + "--" + "Duress Alarm";
					break;
				case 28:
					ret = String.valueOf(logData.dwManipulation) + "--" + "Enroll Photo";
					break;
				case 29:
					ret = String.valueOf(logData.dwManipulation) + "--" + "Delete Photo";
					break;
				case 31:
					ret = String.valueOf(logData.dwManipulation) + "--" + "Tamper Alarm";
					break;
				case 32:
					ret = String.valueOf(logData.dwManipulation) + "--" + "Enter Menu";
					break;
				case 33:
					ret = String.valueOf(logData.dwManipulation) + "--" + "Delete User";
					break;

				case 34:
					ret = String.valueOf(logData.dwManipulation) + "--" + "AbnormalBodyTemperature Alarm";
					break;
					
				default:
					ret = "--Unknown--";
				}
				return ret;
			}
			case 6:
				if (logData.dwBackupNumber < 10)
					return logData.dwBackupNumber;
				else if (logData.dwBackupNumber == 10)
					return "Password";
				else if (logData.dwBackupNumber == 14)
					return "FACE";
				else
					return "Card";
			case 7: {
				NumberFormat formatter = new DecimalFormat("00");
				String ret = String.valueOf(logData.dwYear) + "/" + formatter.format(logData.dwMonth) + "/"
						+ formatter.format(logData.dwDay) + " ";
				ret += formatter.format(logData.dwHour) + ":" + formatter.format(logData.dwMinute) + ":"
						+ formatter.format(logData.dwSecond);
				return ret;
			}
			default:
				return null;
			}
		}

		@Override
		public String getColumnName(int columnIndex) {
			return COLUMN_TEXT[columnIndex];
		}
	}

	class GLogDataListModel extends AbstractTableModel {
		final String[] COLUMN_TEXT = { "PhotoNo", "strEnrollNo", "BodyTemperature", "Mood", "EMachineNo", "VeriMode", "DateTime", "DaiGong", };
		List<GeneralLogData> logList;

		public void addGLogData(GeneralLogData logData) {
			logList.add(logData);
			fireTableRowsInserted(logList.size() - 1, logList.size() - 1);
		}

		public void removeAll() {
			logList.removeAll(logList);
			fireTableDataChanged();
		}

		public GLogDataListModel() {
			logList = new ArrayList<GeneralLogData>();
		}

		public int getRowCount() {
			return logList.size();
		}

		public int getColumnCount() {
			return COLUMN_TEXT.length;
		}

		public Object getValueAt(int rowIndex, int columnIndex) {
			GeneralLogData logData = logList.get(rowIndex);

			int attendStatus, antipassStatus, daigong, verifyMode;

			antipassStatus = (int) logData.dwVerifyMode / 65536;
			daigong = antipassStatus / 4;
			antipassStatus = antipassStatus % 4;
			verifyMode = (int) logData.dwVerifyMode % 65536;
			attendStatus = verifyMode / 256;
			verifyMode = verifyMode % 256;
            while (verifyMode > 50)
            	verifyMode -= 50;

			String strAttendStatus, strAntipassStatus, strVerifyMode;
			switch (attendStatus) {
			case 0:
				strAttendStatus = "_Duty On";
				break;
			case 1:
				strAttendStatus = "_Duty Off";
				break;
			case 2:
				strAttendStatus = "_Overtime On";
				break;
			case 3:
				strAttendStatus = "_Overtime Off";
				break;
			case 4:
				strAttendStatus = "_Go In";
				break;
			case 5:
				strAttendStatus = "_Go Out";
				break;
			default:
				strAttendStatus = "";
			}

			switch (antipassStatus) {
			case 1:
				strAntipassStatus = ":AP_In";
				break;
			case 2:
				strAntipassStatus = ":AP_Out";
				break;
			default:
				strAntipassStatus = "";
			}

			switch (verifyMode) {
			case 0:
				strVerifyMode = "FP+ID";
				break;
			case 1:
				strVerifyMode = "FP";
				break;
			case 2:
				strVerifyMode = "Password";
				break;
			case 3:
				strVerifyMode = "Card";
				break;
			case 4:
				strVerifyMode = "FP+Card";
				break;
			case 5:
				strVerifyMode = "FP+Pwd";
				break;
			case 6:
				strVerifyMode = "Card+Pwd";
				break;
			case 7:
				strVerifyMode = "FP+Card+Pwd";
				break;
			case 10:
				strVerifyMode = "Hand Lock";
				break;
			case 11:
				strVerifyMode = "Prog Lock";
				break;
			case 12:
				strVerifyMode = "Prog Open";
				break;
			case 13:
				strVerifyMode = "Prog Close";
				break;
			case 14:
				strVerifyMode = "Auto Recover";
				break;
			case 20:
				strVerifyMode = "Lock Over";
				break;
			case 21:
				strVerifyMode = "Illegal Open";
				break;
			case 22:
				strVerifyMode = "Duress alarm";
				break;
			case 23:
				strVerifyMode = "Tamper detect";
				break;
            case 30: strVerifyMode = "FACE"; break;
            case 31: strVerifyMode = "FACE+CARD"; break;
            case 32: strVerifyMode = "FACE+PWD"; break;
            case 33: strVerifyMode = "FACE+CARD+PWD"; break;
            case 34: strVerifyMode = "FACE+FP"; break;
			default:
				strVerifyMode = "";
				break;
			}

			switch (columnIndex) {
			case 0:
				return (logData.dwTMachineNumber == -1 ? "No Photo" : logData.dwTMachineNumber);
			case 1:
				return logData.strEnrollNumber;
			case 2:
                final int BodyTemperature100_Glog_Delta0 = 3000;
                int body_temperature_10_delta = (int)(logData.dwGlogExt & 0xFF); // (body_temperature_10_delta: 8, mood: 2, reserved: 22)
                if (body_temperature_10_delta > 0)
                {
                    int body_temperature_100 = body_temperature_10_delta;
                    body_temperature_100 = body_temperature_100 * 10 + BodyTemperature100_Glog_Delta0;
					return String.valueOf(body_temperature_100 / 100) + "." + String.valueOf((body_temperature_100 % 100) / 10) + "'C";
                }
                else
					return "";
			case 3:
				int v = (int)((logData.dwGlogExt >> 8) & 0x3); // (body_temperature_10_delta: 8, mood: 2, reserved: 22)
				if (v > 0)
				{
					String str = String.valueOf(v);
                    if (v == 1)
                        str += " (SAD)";
                    else if (v == 2)
						str += " (NORMAL)";
                    else if (v == 3)
						str += " (HAPPY)";
					return str;
				}
				else
					return "";
			case 4:
				return logData.dwEMachineNumber;
			case 5:
				return strVerifyMode + strAttendStatus + strAntipassStatus;
			case 6: 
				NumberFormat formatter = new DecimalFormat("00");
				String ret = String.valueOf(logData.dwYear) + "/" + formatter.format(logData.dwMonth) + "/"
						+ formatter.format(logData.dwDay) + " ";
				ret += formatter.format(logData.dwHour) + ":" + formatter.format(logData.dwMinute) + ":"
						+ formatter.format(logData.dwSecond);
				return ret;
			case 7:
				return String.valueOf(daigong);
			default:
				return null;
			}
		}

		@Override
		public String getColumnName(int columnIndex) {
			return COLUMN_TEXT[columnIndex];
		}
	}
}
