package smack.comm.sample;

import java.awt.Color;
import java.awt.EventQueue;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.WindowAdapter;
import java.awt.event.WindowEvent;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.sql.SQLException;
import java.text.DecimalFormat;
import java.text.NumberFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.Iterator;
import java.util.List;
import java.util.logging.Level;
import java.util.logging.Logger;

import javax.swing.DefaultComboBoxModel;
import javax.swing.DefaultListModel;
import javax.swing.ImageIcon;
import javax.swing.JButton;
import javax.swing.JCheckBox;
import javax.swing.JComboBox;
import javax.swing.JFileChooser;
import javax.swing.JFormattedTextField;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JList;
import javax.swing.JOptionPane;
import javax.swing.JPanel;
import javax.swing.JTextField;
import javax.swing.SwingConstants;
import javax.swing.SwingWorker;
import javax.swing.border.LineBorder;
import javax.swing.border.TitledBorder;

import smack.comm.SBXPCProxy;
import smack.comm.input.GetEnrollDataInput;
import smack.comm.input.GetUserNameInput;
import smack.comm.input.SetEnrollDataInput;
import smack.comm.input.SetUserNameInput;
import smack.comm.output.GetAllUserIDOutput;
import smack.comm.output.GetEnrollDataOutput;
import smack.comm.output.OneStringOutput;
import smack.comm.output.XMLParseBinaryByteOutput;
import smack.comm.sample.global.EnrollDBUtil;
import smack.comm.sample.global.SysUtil;
import smack.comm.sample.global.UserEnrollData;

import java.awt.Font;

import javax.swing.border.BevelBorder;
import javax.swing.UIManager;

public class EnrollMngFrame extends JFrame {
	/**
	 * Launch the application.
	 */
	public static void main(String[] args) {
		EventQueue.invokeLater(new Runnable() {
			public void run() {
				try {
					EnrollMngFrame frame = new EnrollMngFrame();
					frame.setVisible(true);
				} catch (Exception e) {
					e.printStackTrace();
				}
			}
		});
	}

	private JButton btnClearAllData;
	private JButton btnDeleteDb;
	private JButton btnDeleteEnrollData;
	private JButton btnDeleteUserPhoto;
	private JButton btnEmptyEnrollData;
	private JButton btnEnableUser;
	private JButton btnGetAllEnrollData;
	private JButton btnGetEnrollData;
	private JButton btnGetEnrollInfo;
	private JButton btnGetName;
	private JButton btnGetUserPhoto;
	private JButton btnModifyDuressFp;
	private JButton btnModifyPrivilege;
	private JButton btnRemoteEnrollCard;
	private JButton btnRemoteEnrollFace;
	private JButton btnRemoteEnrollFp;
	private JButton btnExitRemoteEnroll;
	private JButton btnQueryRemoteEnrollStatus;
	private JButton btnSetAllEnrollData;
	private JButton btnSetEnrollData;
	private JButton btnSetName;
	private JButton btnSetUserPhoto;
	private JButton btnUserPhotoBrowse;
	private JCheckBox chkDisableUser;
	private JCheckBox chkShowProgress;
	private JComboBox cmbBackupNumber;
	private JComboBox cmbDuress;
	private JComboBox cmbPrivilege;
	private JFileChooser fileChooserUserPhoto;
	private JLabel lblBackupNumber;
	private JLabel lblDepartment;
	private JLabel lblDisable;
	private JLabel lblDuress;
	private JLabel lblEnrollData;
	private JLabel lblEnrollNumber;
	private JLabel lblFaceFp;
	private JLabel lblMessage;
	private JLabel lblName;
	private JLabel lblPasswordCard;
	private JLabel lblPrivilege;
	private JLabel lblTotal;
	private JLabel lblUserPhoto;
	private JLabel lblUsertz_1;
	private JLabel lblUsertz_2;
	private JLabel lblUsertz_3;
	private JLabel lblUsertz_4;
	private JLabel lblUsertz_5;
	private JList lstEnrollData;
	private JPanel panelUserPhoto;
	private JTextField txtEnrollNum;
	private JTextField txtName;
	private JTextField txtPasswordCard;
	private JTextField txtUserDepart;
	private JTextField txtUserPhotoFile;
	private JTextField txtUserTz1;
	private JTextField txtUserTz2;
	private JTextField txtUserTz3;
	private JTextField txtUserTz4;
	private JTextField txtUserTz5;

	private JCheckBox chkPeriodUse;
	private JFormattedTextField txtStart;
	private JLabel lblPeriodFromTo;
	private JFormattedTextField txtEnd;
	private JButton btnGetUserPeriod;
	private JButton btnSetUserPeriod;
	private JPanel panel;
	private JLabel lblRemoteEnroll;

	/**
	 * Create the frame.
	 */
	public EnrollMngFrame() {
		addWindowListener(new WindowAdapter() {
			@Override
			public void windowClosed(WindowEvent arg0) {
				if (MainFrame.getInstance() != null)
					MainFrame.getInstance().setVisible(true);
			}
		});

		setTitle("Enroll Management");
		setBounds(100, 100, 814, 654);
		setDefaultCloseOperation(javax.swing.WindowConstants.DISPOSE_ON_CLOSE);
		getContentPane().setLayout(null);

		lblEnrollNumber = new JLabel("Enroll Number :");
		lblEnrollNumber.setHorizontalAlignment(SwingConstants.RIGHT);
		lblEnrollNumber.setBounds(0, 63, 112, 14);
		getContentPane().add(lblEnrollNumber);

		txtEnrollNum = new JTextField();
		txtEnrollNum.setText("1");
		txtEnrollNum.setBounds(120, 60, 86, 20);
		getContentPane().add(txtEnrollNum);
		txtEnrollNum.setColumns(10);

		lblName = new JLabel("Name :");
		lblName.setHorizontalAlignment(SwingConstants.LEFT);
		lblName.setBounds(241, 66, 74, 14);
		getContentPane().add(lblName);

		txtName = new JTextField();
		txtName.setColumns(10);
		txtName.setBounds(316, 63, 122, 20);
		getContentPane().add(txtName);

		lblDepartment = new JLabel("Depart No. :");
		lblDepartment.setHorizontalAlignment(SwingConstants.RIGHT);
		lblDepartment.setBounds(0, 116, 112, 14);
		getContentPane().add(lblDepartment);

		txtUserDepart = new JTextField();
		txtUserDepart.setColumns(10);
		txtUserDepart.setBounds(120, 113, 86, 20);
		getContentPane().add(txtUserDepart);

		lblPrivilege = new JLabel("Privilege :");
		lblPrivilege.setHorizontalAlignment(SwingConstants.RIGHT);
		lblPrivilege.setBounds(0, 141, 112, 14);
		getContentPane().add(lblPrivilege);

		cmbPrivilege = new JComboBox();
		cmbPrivilege.setModel(new DefaultComboBoxModel(new String[] { "0", "1", "2" }));
		cmbPrivilege.setBounds(120, 141, 86, 20);
		getContentPane().add(cmbPrivilege);

		lblFaceFp = new JLabel("0-9:fp,11:card,14:tz,15:pwd, 16:depart,17:face ");
		lblFaceFp.setBounds(58, 452, 311, 14);
		getContentPane().add(lblFaceFp);

		lblUsertz_1 = new JLabel("UserTz1 :");
		lblUsertz_1.setHorizontalAlignment(SwingConstants.RIGHT);
		lblUsertz_1.setBounds(0, 176, 112, 14);
		getContentPane().add(lblUsertz_1);

		txtUserTz1 = new JTextField();
		txtUserTz1.setColumns(10);
		txtUserTz1.setBounds(120, 173, 86, 20);
		getContentPane().add(txtUserTz1);

		lblUsertz_2 = new JLabel("UserTz2 :");
		lblUsertz_2.setHorizontalAlignment(SwingConstants.RIGHT);
		lblUsertz_2.setBounds(0, 198, 112, 14);
		getContentPane().add(lblUsertz_2);

		txtUserTz2 = new JTextField();
		txtUserTz2.setColumns(10);
		txtUserTz2.setBounds(120, 195, 86, 20);
		getContentPane().add(txtUserTz2);

		lblUsertz_3 = new JLabel("UserTz3 :");
		lblUsertz_3.setHorizontalAlignment(SwingConstants.RIGHT);
		lblUsertz_3.setBounds(0, 221, 112, 14);
		getContentPane().add(lblUsertz_3);

		txtUserTz3 = new JTextField();
		txtUserTz3.setColumns(10);
		txtUserTz3.setBounds(120, 218, 86, 20);
		getContentPane().add(txtUserTz3);

		lblUsertz_4 = new JLabel("UserTz4 :");
		lblUsertz_4.setHorizontalAlignment(SwingConstants.RIGHT);
		lblUsertz_4.setBounds(0, 243, 112, 14);
		getContentPane().add(lblUsertz_4);

		txtUserTz4 = new JTextField();
		txtUserTz4.setColumns(10);
		txtUserTz4.setBounds(120, 240, 86, 20);
		getContentPane().add(txtUserTz4);

		lblUsertz_5 = new JLabel("UserTz5 :");
		lblUsertz_5.setHorizontalAlignment(SwingConstants.RIGHT);
		lblUsertz_5.setBounds(0, 266, 112, 14);
		getContentPane().add(lblUsertz_5);

		txtUserTz5 = new JTextField();
		txtUserTz5.setColumns(10);
		txtUserTz5.setBounds(120, 263, 86, 20);
		getContentPane().add(txtUserTz5);

		lblDisable = new JLabel("Disable :");
		lblDisable.setHorizontalAlignment(SwingConstants.RIGHT);
		lblDisable.setBounds(0, 330, 112, 14);
		getContentPane().add(lblDisable);
		lblDisable.setLabelFor(chkDisableUser);

		chkDisableUser = new JCheckBox("");
		chkDisableUser.setToolTipText("");
		chkDisableUser.setBounds(120, 326, 21, 23);
		getContentPane().add(chkDisableUser);

		lblDuress = new JLabel("Duress :");
		lblDuress.setHorizontalAlignment(SwingConstants.RIGHT);
		lblDuress.setBounds(0, 304, 112, 14);
		getContentPane().add(lblDuress);

		cmbDuress = new JComboBox();
		cmbDuress.setModel(new DefaultComboBoxModel(new String[] { "0", "1" }));
		cmbDuress.setBounds(120, 301, 86, 20);
		getContentPane().add(cmbDuress);

		lblPasswordCard = new JLabel("Card / Password :");
		lblPasswordCard.setHorizontalAlignment(SwingConstants.RIGHT);
		lblPasswordCard.setBounds(0, 399, 112, 14);
		getContentPane().add(lblPasswordCard);

		txtPasswordCard = new JTextField();
		txtPasswordCard.setColumns(10);
		txtPasswordCard.setBounds(120, 396, 86, 20);
		getContentPane().add(txtPasswordCard);

		lblBackupNumber = new JLabel("Backup Number :");
		lblBackupNumber.setHorizontalAlignment(SwingConstants.RIGHT);
		lblBackupNumber.setBounds(0, 427, 112, 14);
		getContentPane().add(lblBackupNumber);

		cmbBackupNumber = new JComboBox();
		cmbBackupNumber.setModel(new DefaultComboBoxModel(
				new String[] { "0", "1", "2", "10", "11", "12", "13", "14", "15", "16", "17" }));
		cmbBackupNumber.setBounds(120, 427, 86, 20);
		getContentPane().add(cmbBackupNumber);

		lstEnrollData = new JList();
		lstEnrollData.setEnabled(false);
		lstEnrollData.setBorder(new LineBorder(new Color(0, 0, 0)));
		lstEnrollData.setBounds(241, 107, 197, 139);
		getContentPane().add(lstEnrollData);

		lblEnrollData = new JLabel("Enroll Data :");
		lblEnrollData.setBounds(241, 88, 112, 14);
		getContentPane().add(lblEnrollData);

		btnGetEnrollData = new JButton("Get EnrollData");
		btnGetEnrollData.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				boolean ret;
				int errorCode;

				lblMessage.setText(SysUtil.WORKING);
				invalidate();

				ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, false);

				if (!ret) {
					lblMessage.setText(SysUtil.NO_DEVICE);
					return;
				}

				GetEnrollDataInput input = new GetEnrollDataInput();
				input.dwMachineNumber = SysUtil.MachineNumber;
				input.dwEMachineNumber = SysUtil.MachineNumber;
				try {
					input.dwBackupNumber = Long.parseLong(cmbBackupNumber.getSelectedItem().toString());
					input.dwEnrollNumber = Long.parseLong(txtEnrollNum.getText());
				} catch (NumberFormatException ne) {
					ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, true);
					JOptionPane.showMessageDialog(null, "Enroll number format is invalid.");
				}

				if (input.dwBackupNumber == 10)
					input.dwBackupNumber = 15;

				GetEnrollDataOutput output;

				output = SBXPCProxy.GetEnrollData(input);

				if (output.isSuccess()) {
					cmbPrivilege.setSelectedIndex((int) output.dwMachinePrivilege);

					if (input.dwBackupNumber == 11) // Card number
					{
						txtPasswordCard.setText(Long.toHexString(output.dwPassword));
					} else if (input.dwBackupNumber == 14) // User time zone
					{
						txtUserTz1.setText(String.valueOf(output.dwPassword % 64));
						output.dwPassword /= 64;
						txtUserTz2.setText(String.valueOf(output.dwPassword % 64));
						output.dwPassword /= 64;
						txtUserTz3.setText(String.valueOf(output.dwPassword % 64));
						output.dwPassword /= 64;
						txtUserTz4.setText(String.valueOf(output.dwPassword % 64));
						output.dwPassword /= 64;
						txtUserTz5.setText(String.valueOf(output.dwPassword % 64));
					} else if (input.dwBackupNumber == 15) // User password
					{
						String userPassword = "";
						while (output.dwPassword > 0) {
							userPassword += ((output.dwPassword & 0x0F) - 1);
							output.dwPassword /= 16;
						}
						txtPasswordCard.setText(userPassword);
					} else if (input.dwBackupNumber == 16) {
						txtUserDepart.setText(String.valueOf(output.dwPassword));
					} else {
						DefaultListModel model = new DefaultListModel();
						lstEnrollData.setModel(model);
						for (int i = 0; i < output.dwEnrollData.length; i++)
							model.addElement(output.dwEnrollData[i]);
					}
					errorCode = 0;
				} else {
					errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;
				}

				lblMessage.setText(SysUtil.ErrorPrint(errorCode));

				ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, true);
			}
		});
		btnGetEnrollData.setBounds(459, 59, 149, 23);
		getContentPane().add(btnGetEnrollData);

		btnSetEnrollData = new JButton("Set EnrollData");
		btnSetEnrollData.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				boolean ret;
				int errorCode;

				lblMessage.setText(SysUtil.WORKING);
				invalidate();

				ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, false);

				if (!ret) {
					lblMessage.setText(SysUtil.NO_DEVICE);
					return;
				}

				SetEnrollDataInput input = new SetEnrollDataInput();
				input.dwMachineNumber = SysUtil.MachineNumber;
				input.dwEMachineNumber = SysUtil.MachineNumber;
				input.dwMachinePrivilege = cmbPrivilege.getSelectedIndex();
				try {
					input.dwBackupNumber = Long.parseLong(cmbBackupNumber.getSelectedItem().toString());
					input.dwEnrollNumber = Long.parseLong(txtEnrollNum.getText());
				} catch (NumberFormatException ne) {
					ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, true);
					JOptionPane.showMessageDialog(null, "Enroll number format is invalid.");
				}

				if (input.dwBackupNumber == 10)
					input.dwBackupNumber = 15;

				if (input.dwBackupNumber == 11) // Card Number
				{
					try {
						input.dwPassword = Long.parseLong(txtPasswordCard.getText(), 16);
					} catch (NumberFormatException ne) {
						input.dwPassword = 0;
					}
				} else if (input.dwBackupNumber == 14) // User time zone
				{
					try {
						input.dwPassword = Long.parseLong(txtUserTz5.getText());
						input.dwPassword = input.dwPassword * 64 + Long.parseLong(txtUserTz4.getText());
						input.dwPassword = input.dwPassword * 64 + Long.parseLong(txtUserTz3.getText());
						input.dwPassword = input.dwPassword * 64 + Long.parseLong(txtUserTz2.getText());
						input.dwPassword = input.dwPassword * 64 + Long.parseLong(txtUserTz1.getText());
					} catch (NumberFormatException ne) {
						input.dwPassword = 0;
					}

				} else if (input.dwBackupNumber == 15) // User password
				{
					input.dwPassword = 0;
					String pwd = txtPasswordCard.getText();
					int i = Math.min(pwd.length(), 6);
					while (i > 0) {
						input.dwPassword = input.dwPassword * 16 + pwd.charAt(i - 1) - '0' + 1;
						i--;
					}
				} else if (input.dwBackupNumber == 16) // User department
				{
					try {
						input.dwPassword = Long.parseLong(txtUserDepart.getText());
					} catch (NumberFormatException ne) {
						input.dwPassword = 0;
					}
				} else {
					input.dwEnrollData = new int[lstEnrollData.getModel().getSize()];
					for (int i = 0; i < lstEnrollData.getModel().getSize(); i++)
						input.dwEnrollData[i] = Integer.parseInt(lstEnrollData.getModel().getElementAt(i).toString());
				}

				ret = SBXPCProxy.SetEnrollData(input);

				if (ret)
					errorCode = 0;
				else
					errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;

				lblMessage.setText(SysUtil.ErrorPrint(errorCode));

				ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, true);

			}
		});
		btnSetEnrollData.setBounds(618, 59, 149, 23);
		getContentPane().add(btnSetEnrollData);

		btnGetAllEnrollData = new JButton("Get All Enroll Data");
		btnGetAllEnrollData.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				boolean ret;
				int errorCode;

				lblMessage.setText(SysUtil.WORKING);
				invalidate();

				ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, false);

				if (!ret) {
					lblMessage.setText(SysUtil.NO_DEVICE);
					return;
				}

				ret = SBXPCProxy.ReadAllUserID(SysUtil.MachineNumber);

				if (ret) {
					errorCode = 0;

					int count = 0;
					GetAllUserIDOutput userIdOutput;
					GetEnrollDataInput enrollDataInput = new GetEnrollDataInput();
					GetEnrollDataOutput enrollDataOutput;
					UserEnrollData enrollData = new UserEnrollData();
					while (true) {
						userIdOutput = SBXPCProxy.GetAllUserID(SysUtil.MachineNumber);

						if (!userIdOutput.isSuccess())
							break;

						if (userIdOutput.dwBackupNumber >= 50)
							continue;

						enrollDataInput.dwMachineNumber = SysUtil.MachineNumber;
						enrollDataInput.dwEMachineNumber = SysUtil.MachineNumber;
						enrollDataInput.dwEnrollNumber = userIdOutput.dwEnrollNumber;
						enrollDataInput.dwBackupNumber = userIdOutput.dwBackupNumber;

						enrollDataOutput = SBXPCProxy.GetEnrollData(enrollDataInput);

						if (enrollDataOutput.isSuccess()) {
							enrollData.machineNo = (int) SysUtil.MachineNumber;
							enrollData.enrollNo = (int) enrollDataInput.dwEnrollNumber;
							enrollData.fingerNo = (int) enrollDataInput.dwBackupNumber;
							enrollData.password = (int) enrollDataOutput.dwPassword;
							enrollData.privilege = (int) enrollDataOutput.dwMachinePrivilege;
							enrollData.fpData = new int[enrollDataOutput.dwEnrollData.length];
							System.arraycopy(enrollDataOutput.dwEnrollData, 0, enrollData.fpData, 0,
									enrollDataOutput.dwEnrollData.length);
							try {
								EnrollDBUtil.updateEnrollData(enrollData);
								count++;
							} catch (SQLException se) {
								se.printStackTrace();
							}
						} else {
							errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;
							int selOpt = JOptionPane.showConfirmDialog(null, SysUtil.ErrorPrint(errorCode), "Continue?",
									JOptionPane.YES_NO_OPTION);
							if (selOpt == JOptionPane.NO_OPTION)
								break;
						}
					}
					lblTotal.setText("Total: " + count);
				} else
					errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;

				lblMessage.setText(SysUtil.ErrorPrint(errorCode));

				ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, true);
			}
		});
		btnGetAllEnrollData.setBounds(459, 308, 149, 23);
		getContentPane().add(btnGetAllEnrollData);

		btnDeleteEnrollData = new JButton("Delete EnrollData");
		btnDeleteEnrollData.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				boolean ret;
				int errorCode;

				lblMessage.setText(SysUtil.WORKING);
				invalidate();

				ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, false);

				if (!ret) {
					lblMessage.setText(SysUtil.NO_DEVICE);
					return;
				}

				GetEnrollDataInput input = new GetEnrollDataInput();
				input.dwMachineNumber = SysUtil.MachineNumber;
				input.dwEMachineNumber = SysUtil.MachineNumber;
				try {
					input.dwBackupNumber = Long.parseLong(cmbBackupNumber.getSelectedItem().toString());
					input.dwEnrollNumber = Long.parseLong(txtEnrollNum.getText());
				} catch (NumberFormatException ne) {
					ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, true);
					JOptionPane.showMessageDialog(null, "Enroll number format is invalid.");
				}

				if (input.dwBackupNumber == 10)
					input.dwBackupNumber = 15;

				ret = SBXPCProxy.DeleteEnrollData(input);

				if (ret)
					errorCode = 0;
				else
					errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;

				lblMessage.setText(SysUtil.ErrorPrint(errorCode));

				ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, true);
			}
		});
		btnDeleteEnrollData.setBounds(459, 85, 149, 23);
		getContentPane().add(btnDeleteEnrollData);

		btnGetName = new JButton("Get Name");
		btnGetName.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				boolean ret;
				int errorCode;

				lblMessage.setText(SysUtil.WORKING);
				invalidate();

				ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, false);

				if (!ret) {
					lblMessage.setText(SysUtil.NO_DEVICE);
					return;
				}

				GetUserNameInput input = new GetUserNameInput();
				input.dwMachineNumber = SysUtil.MachineNumber;
				try {
					input.dwEnrollNumber = Long.parseLong(txtEnrollNum.getText());
				} catch (NumberFormatException ne) {
					input.dwEnrollNumber = 1;
					txtEnrollNum.setText("1");
				}

				OneStringOutput output;
				output = SBXPCProxy.GetUserName(input);

				if (output.isSuccess()) {
					errorCode = 0;
					txtName.setText(output.value);
				} else
					errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;

				lblMessage.setText(SysUtil.ErrorPrint(errorCode));

				ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, true);
			}
		});
		btnGetName.setBounds(459, 141, 149, 23);
		getContentPane().add(btnGetName);

		btnSetName = new JButton("Set Name");
		btnSetName.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				boolean ret;
				int errorCode;

				lblMessage.setText(SysUtil.WORKING);
				invalidate();

				ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, false);

				if (!ret) {
					lblMessage.setText(SysUtil.NO_DEVICE);
					return;
				}

				SetUserNameInput input = new SetUserNameInput();
				input.dwMachineNumber = SysUtil.MachineNumber;
				input.setUserName(txtName.getText());
				try {
					input.dwEnrollNumber = Long.parseLong(txtEnrollNum.getText());
				} catch (NumberFormatException ne) {
					input.dwEnrollNumber = 1;
					txtEnrollNum.setText("0");
				}

				ret = SBXPCProxy.SetUserName(input);

				if (ret)
					errorCode = 0;
				else
					errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;

				lblMessage.setText(SysUtil.ErrorPrint(errorCode));

				ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, true);
			}
		});
		btnSetName.setBounds(618, 141, 149, 23);
		getContentPane().add(btnSetName);

		btnGetEnrollInfo = new JButton("Get Enroll Info");
		btnGetEnrollInfo.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				boolean ret;
				int errorCode;

				lblMessage.setText(SysUtil.WORKING);
				invalidate();

				ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, false);

				if (!ret) {
					lblMessage.setText(SysUtil.NO_DEVICE);
					return;
				}

				ret = SBXPCProxy.ReadAllUserID(SysUtil.MachineNumber);

				if (ret) {
					errorCode = 0;
					GetAllUserIDOutput output;
					String element = "No.    EnNo   EMNo   Fp   Priv  Enable Duress";
					DefaultListModel listModel = new DefaultListModel();
					listModel.addElement(element);

					NumberFormat format2 = new DecimalFormat("00");
					NumberFormat format3 = new DecimalFormat("000");
					NumberFormat format5 = new DecimalFormat("00000");
					int count = 0;
					while (true) {
						output = SBXPCProxy.GetAllUserID(SysUtil.MachineNumber);
						if (!output.isSuccess())
							break;
						element = format3.format(count) + "   ";
						element += format5.format(output.dwEnrollNumber) + "    ";
						element += format3.format(output.dwEMachineNumber) + "      ";
						element += format2.format(output.dwBackupNumber) + "    ";
						element += String.valueOf(output.dwMachinePrivilege) + "        ";
						element += String.valueOf(output.dwEnable % 256) + "        ";
						element += String.valueOf(output.dwEnable / 256);

						listModel.addElement(element);

						count++;
					}
					lstEnrollData.setModel(listModel);
					lblTotal.setText("Total: " + count);
				} else
					errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;

				lblMessage.setText(SysUtil.ErrorPrint(errorCode));

				ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, true);
			}
		});
		btnGetEnrollInfo.setBounds(459, 340, 149, 23);
		getContentPane().add(btnGetEnrollInfo);

		btnEnableUser = new JButton("Enable User");
		btnEnableUser.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				boolean ret;
				int errorCode;

				lblMessage.setText(SysUtil.WORKING);
				invalidate();

				ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, false);

				if (!ret) {
					lblMessage.setText(SysUtil.NO_DEVICE);
					return;
				}

				GetEnrollDataInput input = new GetEnrollDataInput();
				input.dwMachineNumber = SysUtil.MachineNumber;
				input.dwEMachineNumber = SysUtil.MachineNumber;
				try {
					input.dwEnrollNumber = Long.parseLong(txtEnrollNum.getText());
					input.dwBackupNumber = Long.parseLong(cmbBackupNumber.getSelectedItem().toString());
				} catch (NumberFormatException ne) {
					input.dwEnrollNumber = 1;
					txtEnrollNum.setText("1");
					input.dwBackupNumber = 0;
					cmbBackupNumber.setSelectedIndex(0);
				}

				ret = SBXPCProxy.EnableUser(input, !chkDisableUser.isSelected());

				if (ret)
					errorCode = 0;
				else
					errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;

				lblMessage.setText(SysUtil.ErrorPrint(errorCode));

				ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, true);
			}
		});
		btnEnableUser.setBounds(459, 198, 149, 23);
		getContentPane().add(btnEnableUser);

		btnModifyPrivilege = new JButton("Modify Privilege");
		btnModifyPrivilege.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				boolean ret;
				int errorCode;

				lblMessage.setText(SysUtil.WORKING);
				invalidate();

				ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, false);

				if (!ret) {
					lblMessage.setText(SysUtil.NO_DEVICE);
					return;
				}

				GetEnrollDataInput input = new GetEnrollDataInput();
				input.dwMachineNumber = SysUtil.MachineNumber;
				input.dwEMachineNumber = SysUtil.MachineNumber;
				try {
					input.dwEnrollNumber = Long.parseLong(txtEnrollNum.getText());
					input.dwBackupNumber = Long.parseLong(cmbBackupNumber.getSelectedItem().toString());
				} catch (NumberFormatException ne) {
					input.dwEnrollNumber = 1;
					txtEnrollNum.setText("1");
					input.dwBackupNumber = 0;
					cmbBackupNumber.setSelectedIndex(0);
				}

				ret = SBXPCProxy.ModifyPrivilege(input, cmbPrivilege.getSelectedIndex());

				if (ret)
					errorCode = 0;
				else
					errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;

				lblMessage.setText(SysUtil.ErrorPrint(errorCode));

				ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, true);
			}
		});
		btnModifyPrivilege.setBounds(459, 172, 149, 23);
		getContentPane().add(btnModifyPrivilege);

		btnModifyDuressFp = new JButton("Modify Duress FP");
		btnModifyDuressFp.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				boolean ret;
				int errorCode;

				lblMessage.setText(SysUtil.WORKING);
				invalidate();

				ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, false);

				if (!ret) {
					lblMessage.setText(SysUtil.NO_DEVICE);
					return;
				}

				long dwEnrollNumber;
				long dwBackupNumber;
				try {
					dwEnrollNumber = Long.parseLong(txtEnrollNum.getText());
					dwBackupNumber = Long.parseLong(cmbBackupNumber.getSelectedItem().toString());
				} catch (NumberFormatException ne) {
					dwEnrollNumber = 1;
					txtEnrollNum.setText("1");
					dwBackupNumber = 0;
					cmbBackupNumber.setSelectedIndex(0);
				}

				ret = SBXPCProxy.ModifyDuressFP(SysUtil.MachineNumber, dwEnrollNumber, dwBackupNumber,
						cmbDuress.getSelectedIndex());

				if (ret)
					errorCode = 0;
				else
					errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;

				lblMessage.setText(SysUtil.ErrorPrint(errorCode));

				ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, true);
			}
		});
		btnModifyDuressFp.setBounds(618, 172, 149, 23);
		getContentPane().add(btnModifyDuressFp);

		panel = new JPanel();
		panel.setLayout(null);
		panel.setBorder(new TitledBorder(UIManager.getBorder("TitledBorder.border"), "Remote Enroll", TitledBorder.LEADING, TitledBorder.TOP, null, new Color(0, 0, 0)));
		panel.setBounds(459, 410, 308, 194);
		getContentPane().add(panel);
		
		btnRemoteEnrollFace = new JButton("Remote Enroll Face");
		btnRemoteEnrollFace.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				RemoteEnroll_actionPerformed(arg0, "RemoteEnrollFace");
			}
		});
		btnRemoteEnrollFace.setBounds(10, 38, 149, 23);
		panel.add(btnRemoteEnrollFace);
		
		btnRemoteEnrollFp = new JButton("Remote Enroll FP");
		btnRemoteEnrollFp.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				RemoteEnroll_actionPerformed(arg0, "RemoteEnrollFP");
			}
		});
		btnRemoteEnrollFp.setBounds(10, 62, 149, 23);
		panel.add(btnRemoteEnrollFp);
		
		btnRemoteEnrollCard = new JButton("Remote Enroll Card");
		btnRemoteEnrollCard.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				RemoteEnroll_actionPerformed(arg0, "RemoteEnrollCard");
			}
		});
		btnRemoteEnrollCard.setBounds(10, 87, 149, 23);
		panel.add(btnRemoteEnrollCard);
								
		btnExitRemoteEnroll = new JButton("Exit Remote Enroll");
		btnExitRemoteEnroll.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				lblMessage.setText(SysUtil.WORKING);
				invalidate();

				int errorCode;
				OneStringOutput output;
				output = SysUtil.MakeXMLCommandHeader("ExitRemoteEnroll");

				output = SBXPCProxy.GeneralOperationXML(SysUtil.MachineNumber, output.value);

				if (output.isSuccess()) {
					String strResultCode = SBXPCProxy.XML_ParseString(output.value, "ResultCode").value;
					if (strResultCode.equals("SuccessExitRemoteEnroll"))
						lblMessage.setText("Success Exit RemoteEnroll.");
					else if (strResultCode.equals("NotStartedRemoteEnroll"))
						lblMessage.setText("Remote Enroll is not Started.");
					else
						lblMessage.setText("Unknown Error");
				} else {
					errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;
					lblMessage.setText(SysUtil.ErrorPrint(errorCode));
				}
			}
		});
		btnExitRemoteEnroll.setBounds(10, 112, 149, 23);
		panel.add(btnExitRemoteEnroll);
										
		btnQueryRemoteEnrollStatus = new JButton("<html>Query RemoteEnroll<br>Status</html>");
		btnQueryRemoteEnrollStatus.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				lblMessage.setText(SysUtil.WORKING);
				invalidate();

				int errorCode;
				OneStringOutput output;
				output = SysUtil.MakeXMLCommandHeader("QueryRemoteEnrollStatus");

				output = SBXPCProxy.GeneralOperationXML(SysUtil.MachineNumber, output.value);

				if (output.isSuccess()) {
					String strResultCode = SBXPCProxy.XML_ParseString(output.value, "ResultCode").value;
					if (strResultCode.equals("RemoteEnrollAlreadyStarted"))
						lblMessage.setText("[QueryRemoteEnrollStatus] RemoteEnrollAlreadyStarted");
					else if (strResultCode.equals("NotStartedRemoteEnroll"))
						lblMessage.setText("[QueryRemoteEnrollStatus] NotStartedRemoteEnroll");
					else
						lblMessage.setText("Unknown Error");
				} else {
					errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;
					lblMessage.setText(SysUtil.ErrorPrint(errorCode));
				}
			}
		});
		btnQueryRemoteEnrollStatus.setBounds(10, 140, 149, 43);
		panel.add(btnQueryRemoteEnrollStatus);
		
		lblRemoteEnroll = new JLabel("");
		lblRemoteEnroll.setBorder(new BevelBorder(BevelBorder.LOWERED, null, null, null, null));
		lblRemoteEnroll.setBounds(163, 21, 135, 162);
		panel.add(lblRemoteEnroll);
												
		chkShowProgress = new JCheckBox("Show Progress");
		chkShowProgress.setBounds(10, 15, 150, 23);
		panel.add(chkShowProgress);
												
		JButton btnGetCompanyName = new JButton("Get Company Name");
		btnGetCompanyName.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				boolean ret;
				int errorCode;

				lblMessage.setText(SysUtil.WORKING);
				invalidate();

				ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, false);

				if (!ret) {
					lblMessage.setText(SysUtil.NO_DEVICE);
					return;
				}

				OneStringOutput output;
				output = SBXPCProxy.GetCompanyName(SysUtil.MachineNumber);

				if (output.isSuccess()) {
					errorCode = 0;
					txtName.setText(output.value);
				} else
					errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;

				lblMessage.setText(SysUtil.ErrorPrint(errorCode));

				ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, true);
			}
		});
		btnGetCompanyName.setBounds(459, 116, 149, 23);
		getContentPane().add(btnGetCompanyName);
												
		JButton btnSetCompanyName = new JButton("Set Company Name");
		btnSetCompanyName.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				boolean ret;
				int errorCode;

				lblMessage.setText(SysUtil.WORKING);
				invalidate();

				ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, false);

				if (!ret) {
					lblMessage.setText(SysUtil.NO_DEVICE);
					return;
				}

				ret = SBXPCProxy.SetCompanyName(SysUtil.MachineNumber, 1, txtName.getText());

				if (ret)
					errorCode = 0;
				else
					errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;

				lblMessage.setText(SysUtil.ErrorPrint(errorCode));

				ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, true);
			}
		});
		btnSetCompanyName.setBounds(618, 116, 149, 23);
		getContentPane().add(btnSetCompanyName);
											
		JButton btnDeleteCompanyName = new JButton("Delete Company Name");
		btnDeleteCompanyName.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				boolean ret;
				int errorCode;

				lblMessage.setText(SysUtil.WORKING);
				invalidate();

				ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, false);

				if (!ret) {
					lblMessage.setText(SysUtil.NO_DEVICE);
					return;
				}

				ret = SBXPCProxy.SetCompanyName(SysUtil.MachineNumber, 0, "");

				if (ret)
					errorCode = 0;
				else
					errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;

				lblMessage.setText(SysUtil.ErrorPrint(errorCode));

				ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, true);
			}
		});
		btnDeleteCompanyName.setBounds(618, 87, 149, 23);
		getContentPane().add(btnDeleteCompanyName);
												
		JButton btnEnrollFaceFromPhoto = new JButton("Enroll Face from Photo");
		btnEnrollFaceFromPhoto.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				long userId;
				try {
					userId = Long.parseLong(txtEnrollNum.getText());
				} catch (NumberFormatException ne) {
					JOptionPane.showMessageDialog(null, "Invalid Enroll Number");
					return;
				}
										
				byte[] photoData;
				int PhotoSize = 0;
										
				FileInputStream fis = null;
				try {
					fis = new FileInputStream(txtUserPhotoFile.getText());
					PhotoSize = fis.available();
					if (PhotoSize > SysUtil.COMPRESSED_PHOTO_SIZE_32K)
						return;
					photoData = new byte[PhotoSize];
					fis.read(photoData);
				} catch (IOException ex) {
					JOptionPane.showMessageDialog(null, "Invalid photo file.");
					return;
				} finally {
					if (fis != null) {
						try {
							fis.close();
						} catch (IOException ex) {
						}
					}
				}
										
				boolean ret;
				int errorCode;
										
				lblMessage.setText(SysUtil.WORKING);
				invalidate();
										
				ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, false);
										
				if (!ret) {
					lblMessage.setText(SysUtil.NO_DEVICE);
					return;
				}
										
				OneStringOutput output;
				output = SysUtil.MakeXMLCommandHeader("EnrollFaceByPhoto");
				output = SBXPCProxy.XML_AddLong(output.value, "UserID", userId);
				output = SBXPCProxy.XML_AddLong(output.value, "PhotoSize", PhotoSize);
				output = SBXPCProxy.XML_AddBinaryByte(output.value, "PhotoData", photoData, PhotoSize);
										
				output = SBXPCProxy.GeneralOperationXML(SysUtil.MachineNumber, output.value);
										
				if (output.isSuccess())
				{
					String strResultCode = SBXPCProxy.XML_ParseString(output.value, "ResultCode").value;
					String strErr = SBXPCProxy.XML_ParseString(output.value, "ErrStr").value;
															
					if (strResultCode.equals("Success"))
					{
						lblMessage.setText("EnrollFaceByPhoto OK");
					}
					else
					{
						lblMessage.setText("EnrollFaceByPhoto Failed. (" + strResultCode
								+ ((strErr.length() > 0) ? (": " + strErr) : "")
								+ ")");
					}
				}
				else
				{
					errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;
					lblMessage.setText(SysUtil.ErrorPrint(errorCode));
				}
										
				ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, true);
			}
		});
		btnEnrollFaceFromPhoto.setBounds(618, 250, 149, 23);
		getContentPane().add(btnEnrollFaceFromPhoto);
		
		JButton btnEnrollInfoCustom1 = new JButton("Enroll Info Custom1");
		btnEnrollInfoCustom1.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				EnrollCustom1Frame frame = new EnrollCustom1Frame();
				frame.setVisible(true);
				setVisible(false);
			}
		});
		btnEnrollInfoCustom1.setBounds(289, 562, 149, 23);
		getContentPane().add(btnEnrollInfoCustom1);
		
		btnEmptyEnrollData = new JButton("Empty Enroll Data");
		btnEmptyEnrollData.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				boolean ret;
				int errorCode;

				lblMessage.setText(SysUtil.WORKING);
				invalidate();

				ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, false);

				if (!ret) {
					lblMessage.setText(SysUtil.NO_DEVICE);
					return;
				}

				ret = SBXPCProxy.EmptyEnrollData(SysUtil.MachineNumber);

				if (ret)
					errorCode = 0;
				else
					errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;

				lblMessage.setText(SysUtil.ErrorPrint(errorCode));

				ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, true);
			}
		});
		btnEmptyEnrollData.setBounds(618, 340, 149, 23);
		getContentPane().add(btnEmptyEnrollData);

		btnClearAllData = new JButton("Clear All Data(E, GL, SL)");
		btnClearAllData.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				boolean ret;
				int errorCode;

				lblMessage.setText(SysUtil.WORKING);
				invalidate();

				ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, false);

				if (!ret) {
					lblMessage.setText(SysUtil.NO_DEVICE);
					return;
				}

				ret = SBXPCProxy.ClearKeeperData(SysUtil.MachineNumber);

				if (ret)
					errorCode = 0;
				else
					errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;

				lblMessage.setText(SysUtil.ErrorPrint(errorCode));

				ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, true);
			}
		});
		btnClearAllData.setBounds(459, 374, 149, 23);
		getContentPane().add(btnClearAllData);

		btnDeleteDb = new JButton("Delete DB");
		btnDeleteDb.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				try {
					EnrollDBUtil.deleteAllEnrollData(SysUtil.MachineNumber);
					lblTotal.setText("Total: 0");
					lblMessage.setText("Delete DB Success.");
				} catch (SQLException ex) {
					Logger.getLogger(EnrollMngFrame.class.getName()).log(Level.SEVERE, null, ex);
				}
			}
		});
		btnDeleteDb.setBounds(618, 374, 149, 23);
		getContentPane().add(btnDeleteDb);

		lblMessage = new JLabel("Message");
		lblMessage.setHorizontalAlignment(SwingConstants.CENTER);
		lblMessage.setFont(new Font("Segoe UI", Font.BOLD, 18));
		lblMessage.setBorder(javax.swing.BorderFactory.createBevelBorder(javax.swing.border.BevelBorder.LOWERED));
		lblMessage.setBounds(27, 13, 740, 37);
		getContentPane().add(lblMessage);

		lblTotal = new JLabel("Total :");
		lblTotal.setHorizontalAlignment(SwingConstants.LEFT);
		lblTotal.setBounds(496, 288, 112, 14);
		getContentPane().add(lblTotal);

		panelUserPhoto = new JPanel();
		panelUserPhoto
				.setBorder(new TitledBorder(null, "UserPhoto", TitledBorder.LEADING, TitledBorder.TOP, null, null));
		panelUserPhoto.setBounds(241, 261, 197, 188);
		getContentPane().add(panelUserPhoto);
		panelUserPhoto.setLayout(null);

		txtUserPhotoFile = new JTextField();
		txtUserPhotoFile.setText("D:\\TempPhoto.jpg");
		txtUserPhotoFile.setBounds(12, 155, 143, 20);
		panelUserPhoto.add(txtUserPhotoFile);
		txtUserPhotoFile.setColumns(10);

		fileChooserUserPhoto = new javax.swing.JFileChooser();
		btnUserPhotoBrowse = new JButton("...");
		btnUserPhotoBrowse.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				fileChooserUserPhoto.showOpenDialog(null);
				File selectedFile = fileChooserUserPhoto.getSelectedFile();

				if (selectedFile == null)
					return;
				txtUserPhotoFile.setText(selectedFile.getPath());

				lblUserPhoto.setIcon(new ImageIcon(txtUserPhotoFile.getText()));
			}
		});
		btnUserPhotoBrowse.setBounds(158, 154, 24, 21);
		panelUserPhoto.add(btnUserPhotoBrowse);

		lblUserPhoto = new JLabel("");
		lblUserPhoto.setBorder(javax.swing.BorderFactory.createBevelBorder(javax.swing.border.BevelBorder.LOWERED));
		lblUserPhoto.setBounds(12, 21, 170, 130);
		panelUserPhoto.add(lblUserPhoto);

		btnGetUserPhoto = new JButton("Get User Photo");
		btnGetUserPhoto.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				boolean ret;
				int errorCode;

				lblMessage.setText(SysUtil.WORKING);
				invalidate();

				ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, false);

				if (!ret) {
					lblMessage.setText(SysUtil.NO_DEVICE);
					return;
				}

				lblUserPhoto.setIcon(null);

				long userId;
				try {
					userId = Long.parseLong(txtEnrollNum.getText());
				} catch (NumberFormatException ne) {
					userId = 0;
				}

				OneStringOutput output;
				output = SysUtil.MakeXMLCommandHeader("GetUserPhotoData");
				output = SBXPCProxy.XML_AddLong(output.value, "UserID", userId);

				output = SBXPCProxy.GeneralOperationXML(SysUtil.MachineNumber, output.value);

				if (output.isSuccess()) {
					XMLParseBinaryByteOutput parseOutput;
					int photo_size = SBXPCProxy.XML_ParseInt(output.value, "PhotoSize");
					if (photo_size <= 0)
						photo_size = SysUtil.COMPRESSED_PHOTO_SIZE_8K;
					parseOutput = SBXPCProxy.XML_ParseBinaryByte(output.value, "PhotoData", photo_size);
					if (parseOutput.isSuccess())
					{
						lblUserPhoto.setIcon(new ImageIcon(parseOutput.pData));
						
						FileOutputStream fis = null;
						try {
							fis = new FileOutputStream(txtUserPhotoFile.getText());
							fis.write(parseOutput.pData);
						} catch (IOException ex) {
							JOptionPane.showMessageDialog(null, "Failed to write photo file.");
							return;
						} finally {
							if (fis != null) {
								try {
									fis.close();
								} catch (IOException ex) {
								}
							}
						}
					}
					errorCode = 0;
				} else
					errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;

				lblMessage.setText(SysUtil.ErrorPrint(errorCode));

				ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, true);
			}
		});
		btnGetUserPhoto.setBounds(459, 224, 149, 23);
		getContentPane().add(btnGetUserPhoto);

		btnSetUserPhoto = new JButton("Set User Photo");
		btnSetUserPhoto.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				long userId;
				try {
					userId = Long.parseLong(txtEnrollNum.getText());
				} catch (NumberFormatException ne) {
					JOptionPane.showMessageDialog(null, "Invalid Enroll Number");
					return;
				}

				byte[] userPhotoData;
				int PhotoSize = 0;

				FileInputStream fis = null;
				try {
					fis = new FileInputStream(txtUserPhotoFile.getText());
					PhotoSize = fis.available();
					if (PhotoSize > SysUtil.COMPRESSED_PHOTO_SIZE_32K)
						return;
					userPhotoData = new byte[PhotoSize];
					fis.read(userPhotoData);
				} catch (IOException ex) {
					JOptionPane.showMessageDialog(null, "Invalid user photo file.");
					return;
				} finally {
					if (fis != null) {
						try {
							fis.close();
						} catch (IOException ex) {
						}
					}
				}

				boolean ret;
				int errorCode;

				lblMessage.setText(SysUtil.WORKING);
				invalidate();

				ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, false);

				if (!ret) {
					lblMessage.setText(SysUtil.NO_DEVICE);
					return;
				}

				OneStringOutput output;
				output = SysUtil.MakeXMLCommandHeader("SetUserPhotoData");
				output = SBXPCProxy.XML_AddLong(output.value, "UserID", userId);
				output = SBXPCProxy.XML_AddLong(output.value, "PhotoSize", PhotoSize);
				output = SBXPCProxy.XML_AddBinaryByte(output.value, "PhotoData", userPhotoData, PhotoSize);

				output = SBXPCProxy.GeneralOperationXML(SysUtil.MachineNumber, output.value);

				if (output.isSuccess())
					errorCode = 0;
				else
					errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;

				lblMessage.setText(SysUtil.ErrorPrint(errorCode));

				ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, true);
			}
		});
		btnSetUserPhoto.setBounds(618, 221, 149, 23);
		getContentPane().add(btnSetUserPhoto);

		btnDeleteUserPhoto = new JButton("Delete User Photo");
		btnDeleteUserPhoto.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				long userId;
				try {
					userId = Long.parseLong(txtEnrollNum.getText());
				} catch (NumberFormatException ne) {
					return;
				}

				boolean ret;
				int errorCode;

				lblMessage.setText(SysUtil.WORKING);
				invalidate();

				ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, false);

				if (!ret) {
					lblMessage.setText(SysUtil.NO_DEVICE);
					return;
				}

				OneStringOutput output;
				output = SysUtil.MakeXMLCommandHeader("SetUserPhotoData");
				output = SBXPCProxy.XML_AddLong(output.value, "UserID", userId);
				// Did not set "PhotoData" field to delete user photo

				output = SBXPCProxy.GeneralOperationXML(SysUtil.MachineNumber, output.value);

				if (output.isSuccess())
					errorCode = 0;
				else
					errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;

				lblMessage.setText(SysUtil.ErrorPrint(errorCode));

				ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, true);
			}
		});
		btnDeleteUserPhoto.setBounds(459, 250, 149, 23);
		getContentPane().add(btnDeleteUserPhoto);

		btnSetAllEnrollData = new JButton("Set All Enroll Data");
		btnSetAllEnrollData.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				List<UserEnrollData> enrollDataList = new ArrayList<UserEnrollData>();
				try {
					EnrollDBUtil.getAllEnrollData(SysUtil.MachineNumber, enrollDataList);
				} catch (SQLException ex) {
					Logger.getLogger(EnrollMngFrame.class.getName()).log(Level.SEVERE, null, ex);
					return;
				}

				boolean ret;
				int errorCode = 0;

				lblMessage.setText(SysUtil.WORKING);
				invalidate();

				ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, false);

				if (!ret) {
					lblMessage.setText(SysUtil.NO_DEVICE);
					return;
				}

				Iterator<UserEnrollData> iter = enrollDataList.iterator();
				UserEnrollData enrollData;
				SetEnrollDataInput input;
				while (iter.hasNext()) {
					input = new SetEnrollDataInput();
					enrollData = iter.next();
					input.dwMachineNumber = enrollData.machineNo;
					input.dwEMachineNumber = enrollData.machineNo;
					input.dwEnrollNumber = enrollData.enrollNo;
					input.dwBackupNumber = enrollData.fingerNo;
					input.dwMachinePrivilege = enrollData.privilege;
					input.dwPassword = enrollData.password;
					input.dwEnrollData = enrollData.fpData;

					ret = SBXPCProxy.SetEnrollData(input);

					if (ret)
						errorCode = 0;
					else
						errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;
				}

				lblMessage.setText(SysUtil.ErrorPrint(errorCode));

				ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, true);
			}
		});
		btnSetAllEnrollData.setBounds(618, 308, 149, 23);
		getContentPane().add(btnSetAllEnrollData);

		
		chkPeriodUse = new JCheckBox("Use Period");
		chkPeriodUse.setToolTipText("");
		chkPeriodUse.setBounds(98, 485, 100, 20);
		chkPeriodUse.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				if (chkPeriodUse.isSelected())
				{
					txtStart.setEnabled(true);
					txtEnd.setEnabled(true);
				}
				else
				{
					txtStart.setEnabled(false);
					txtEnd.setEnabled(false);
				}
			}
		});
		chkPeriodUse.setSelected(true);
		getContentPane().add(chkPeriodUse);

		txtStart = new JFormattedTextField();
		txtStart.setBounds(228, 485, 100, 20);
        txtStart.setFormatterFactory(new javax.swing.text.DefaultFormatterFactory(new javax.swing.text.DateFormatter(new java.text.SimpleDateFormat("yyyy/MM/dd"))));
        getContentPane().add(txtStart);
       
		lblPeriodFromTo = new JLabel("~");
		lblPeriodFromTo.setHorizontalAlignment(SwingConstants.CENTER);
		lblPeriodFromTo.setBounds(328, 485, 10, 20);
		getContentPane().add(lblPeriodFromTo);

		txtEnd = new JFormattedTextField();
		txtEnd.setBounds(338, 485, 100, 20);
		txtEnd.setFormatterFactory(new javax.swing.text.DefaultFormatterFactory(new javax.swing.text.DateFormatter(new java.text.SimpleDateFormat("yyyy/MM/dd"))));
        getContentPane().add(txtEnd);
        
		btnGetUserPeriod = new JButton("GetUserPeriod");
		btnGetUserPeriod.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				long userId;
				try {
					userId = Long.parseLong(txtEnrollNum.getText());
				} catch (NumberFormatException ne) {
					JOptionPane.showMessageDialog(null, "Invalid Enroll Number");
					return;
				}

				lblMessage.setText(SysUtil.WORKING);
				invalidate();

				int errorCode;
				OneStringOutput output;
				output = SysUtil.MakeXMLCommandHeader("GetUserPeriod");
				output = SBXPCProxy.XML_AddLong(output.value, "UserID", userId);

				output = SBXPCProxy.GeneralOperationXML(SysUtil.MachineNumber, output.value);

				if (output.isSuccess()) {
					boolean Used = (SBXPCProxy.XML_ParseInt(output.value, "Used") != 0);
					int lStartDate = SBXPCProxy.XML_ParseInt(output.value, "Start");
					int lEndDate = SBXPCProxy.XML_ParseInt(output.value, "End");
					
					chkPeriodUse.setSelected(Used);
					if (Used)
					{
						txtStart.setEnabled(true);
						txtEnd.setEnabled(true);
						
			            int yy, mm, dd;

	                    yy = lStartDate / 256 / 256;
	                    mm = (lStartDate - yy * 256 * 256) / 256;
	                    dd = lStartDate & 0xFF;	                    
	                    
	                    Calendar calendar = Calendar.getInstance();
	                    calendar.set(Calendar.YEAR, yy + 2000);
	                    calendar.set(Calendar.MONTH, mm - 1);
	                    calendar.set(Calendar.DAY_OF_MONTH, dd);
	                    txtStart.setValue(calendar.getTime());

	                    yy = lEndDate / 256 / 256;
	                    mm = (lEndDate - yy * 256 * 256) / 256;
	                    dd = lEndDate & 0xFF;
	                    
	                    calendar.set(Calendar.YEAR, yy + 2000);
	                    calendar.set(Calendar.MONTH, mm - 1);
	                    calendar.set(Calendar.DAY_OF_MONTH, dd);
	                    txtEnd.setValue(calendar.getTime());
					}
					else
					{
						txtStart.setEnabled(false);
						txtEnd.setEnabled(false);
					}
					lblMessage.setText("Success");
				} else {
					errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;
					lblMessage.setText(SysUtil.ErrorPrint(errorCode));
				}
			}
		});
		btnGetUserPeriod.setBounds(98, 512, 149, 23);
		getContentPane().add(btnGetUserPeriod);

		btnSetUserPeriod = new JButton("SetUserPeriod");
		btnSetUserPeriod.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				long userId;
				try {
					userId = Long.parseLong(txtEnrollNum.getText());
				} catch (NumberFormatException ne) {
					JOptionPane.showMessageDialog(null, "Invalid Enroll Number");
					return;
				}

				lblMessage.setText(SysUtil.WORKING);
				invalidate();

				int errorCode;
				OneStringOutput output;
				output = SysUtil.MakeXMLCommandHeader("SetUserPeriod");
				output = SBXPCProxy.XML_AddLong(output.value, "UserID", userId);
				output = SBXPCProxy.XML_AddInt(output.value, "Used", chkPeriodUse.isSelected() ? 1 : 0);
				
	            int lStartPeriod, lEndPeriod;
	            if (chkPeriodUse.isSelected())
	            {
	                Calendar calendar = Calendar.getInstance();
	                
	                calendar.setTime((Date)txtStart.getValue());
	                lStartPeriod = (calendar.get(Calendar.YEAR) - 2000) * 256 * 256 + (calendar.get(Calendar.MONTH) + 1) * 256 + calendar.get(Calendar.DAY_OF_MONTH);

	                calendar.setTime((Date)txtEnd.getValue());
	                lEndPeriod = (calendar.get(Calendar.YEAR) - 2000) * 256 * 256 + (calendar.get(Calendar.MONTH) + 1) * 256 + calendar.get(Calendar.DAY_OF_MONTH);
	            }
	            else
	            {
	                lStartPeriod = 1 * 256 + 1;
	                lEndPeriod = 1 * 256 + 1;
	            }
				output = SBXPCProxy.XML_AddInt(output.value, "Start", lStartPeriod);
				output = SBXPCProxy.XML_AddInt(output.value, "End", lEndPeriod);

				output = SBXPCProxy.GeneralOperationXML(SysUtil.MachineNumber, output.value);

				if (output.isSuccess()) {
					lblMessage.setText("Success");
				} else {
					errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;
					lblMessage.setText(SysUtil.ErrorPrint(errorCode));
				}
			}
		});
		btnSetUserPeriod.setBounds(289, 512, 149, 23);
		getContentPane().add(btnSetUserPeriod);
	}

	public void RemoteEnroll_actionPerformed(ActionEvent arg0, String Backup) {
		long userId;
		try {
			userId = Long.parseLong(txtEnrollNum.getText());
		} catch (NumberFormatException ne) {
			JOptionPane.showMessageDialog(null, "Invalid Enroll Number");
			return;
		}

		lblMessage.setText(SysUtil.WORKING);
		invalidate();

		int errorCode;
		OneStringOutput output;
		output = SysUtil.MakeXMLCommandHeader("RemoteEnroll");
		output = SBXPCProxy.XML_AddLong(output.value, "UserID", userId);
		output = SBXPCProxy.XML_AddString(output.value, "Backup", Backup);
		output = SBXPCProxy.XML_AddBoolean(output.value, "ShowProgress", chkShowProgress.isSelected());

		output = SBXPCProxy.GeneralOperationXML(SysUtil.MachineNumber, output.value);
		boolean bSuccess = false;
		if (output.isSuccess()) {
			String strResultCode = SBXPCProxy.XML_ParseString(output.value, "ResultCode").value;
			if (strResultCode.equals("MenuProcessing"))
				lblMessage.setText("Machine is now processing menu.");
			else if (strResultCode.equals("InvalidBackup"))
				lblMessage.setText("Invalid Remote Enroll Backup");
			else if (strResultCode.equals("EnrollNumberError"))
				lblMessage.setText("Invalid Enroll Number");
			else if (strResultCode.equals("FaceAlreadyEnrolled"))
				lblMessage.setText("Face already are enrolled for this user");
			else if (strResultCode.equals("FPAllEnrolled"))
				lblMessage.setText("All fingerprints are enrolled for this user");
			else if (strResultCode.equals("CardAlreadyEnrolled"))
				lblMessage.setText("Card already are enrolled for this user");
			else if (strResultCode.equals("DatabaseFull"))
				lblMessage.setText("Fingerprint database is full.");
			else if (strResultCode.equals("RemoteEnrollAlreadyStarted"))
				lblMessage.setText("Remote Enroll Already Started.");
			else if (strResultCode.equals("Success"))
			{
				bSuccess = true;
				lblMessage.setText("Remote Enroll Started.");
			}
			else
				lblMessage.setText("Unknown Error");
		} else {
			errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;
			lblMessage.setText(SysUtil.ErrorPrint(errorCode));
		}
		
		if (bSuccess && chkShowProgress.isSelected())
		{
			remoteEnrollProgressProc();
		}
	}
	
	
	final int RemoteEnrollStatusNone        = 0;
    final int RemoteEnrollStatusSuccess     = 1;
    final int RemoteEnrollStatusFailed      = 2;
    final int RemoteEnrollStatusProgress    = 3;

    final int RemoteEnrollErrorOk               = 0;
    final int RemoteEnrollErrorDuplicated       = 1;
    final int RemoteEnrollErrorCaptureFailed    = 2;
    final int RemoteEnrollErrorExtractFailed    = 3;
    final int RemoteEnrollErrorMergeFailed      = 4;
    final int RemoteEnrollErrorSaveFailed       = 5;
    final int RemoteEnrollErrorCancelled        = 6;

    private void showErrorMessage(int error)
    {
        if (error == RemoteEnrollErrorOk)
        {
        }
        else if (error == RemoteEnrollErrorDuplicated)
            lblMessage.setText("Duplicated");
        else if (error == RemoteEnrollErrorCaptureFailed)
            lblMessage.setText("Capture Failed");
        else if (error == RemoteEnrollErrorExtractFailed)
            lblMessage.setText("Extract Template Failed");
        else if (error == RemoteEnrollErrorMergeFailed)
            lblMessage.setText("Merge Templates Failed");
        else if (error == RemoteEnrollErrorSaveFailed)
            lblMessage.setText("Save Failed");
        else if (error == RemoteEnrollErrorCancelled)
            lblMessage.setText("Remote Enroll Cancelled");
        else
            lblMessage.setText("Unknown Error");
    }
    
    private void remoteEnrollProgressProc()
    {
    	SwingWorker sw1 = new SwingWorker() {

			@Override
			protected Object doInBackground() throws Exception {
				// TODO Auto-generated method stub
    	
		        boolean bContinue = true;
		        while (bContinue)
		        {
		            invalidate();
		
		    		OneStringOutput output;
		    		output = SysUtil.MakeXMLCommandHeader("RemoteEnrollGetProgress");
		
		    		output = SBXPCProxy.GeneralOperationXML(SysUtil.MachineNumber, output.value);
		            bContinue = false;
		
		            if (output.isSuccess())
		            {
		                String strResultCode = SBXPCProxy.XML_ParseString(output.value, "ResultCode").value;
		                if (strResultCode.equals("RemoteEnrollAlreadyStarted"))
		                {
		                    int status = SBXPCProxy.XML_ParseInt(output.value, "Status");
		                    int error = SBXPCProxy.XML_ParseInt(output.value, "ErrorCode");
		
		                    if (status == RemoteEnrollStatusSuccess)
		                    {
		                        lblMessage.setText("Remote Enroll Success");
		                    }
		                    else if (status == RemoteEnrollStatusFailed)
		                    {
		                        showErrorMessage(error);
		                    }
		                    else if (status == RemoteEnrollStatusNone)
		                    {
		                        showErrorMessage(error);
		                        bContinue = true;
		                    }
		                    else if (status == RemoteEnrollStatusProgress)
		                    {
		                        int progress = SBXPCProxy.XML_ParseInt(output.value, "Progress");
		                        lblMessage.setText("Enroll Progress " + progress);
		
		                        int nImageSize = SBXPCProxy.XML_ParseInt(output.value, "ImageSize");
		                        if (nImageSize > 0)
		                        {
		        					XMLParseBinaryByteOutput parseOutput;
		                        	parseOutput = SBXPCProxy.XML_ParseBinaryByte(output.value, "ImageData", nImageSize);
		                        	if (parseOutput.isSuccess())
		                        	{
		                        		ImageIcon img = new ImageIcon(parseOutput.pData);
		                        		lblRemoteEnroll.setIcon(new ImageIcon(img.getImage().getScaledInstance((int)lblRemoteEnroll.getBounds().getWidth(), (int)lblRemoteEnroll.getBounds().getHeight(), BufferedImage.SCALE_SMOOTH)));
		                        	}
		                        }
		                        bContinue = true;
		                    }
		                }
		                else
		                {
		                    lblMessage.setText(strResultCode);
		                }
		            }
		            else
		            {
		    			int errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;
		    			lblMessage.setText(SysUtil.ErrorPrint(errorCode));
		            }
		        }
				return null;
			}
		};
		
		sw1.execute();
    }
}
