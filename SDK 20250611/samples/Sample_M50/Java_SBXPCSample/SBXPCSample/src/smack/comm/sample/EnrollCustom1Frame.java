package smack.comm.sample;

import java.awt.EventQueue;
import java.awt.Font;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.WindowAdapter;
import java.awt.event.WindowEvent;
import java.util.Base64;

import javax.swing.JButton;
import javax.swing.JCheckBox;
import javax.swing.JFormattedTextField;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JOptionPane;
import javax.swing.JTextArea;
import javax.swing.JTextField;
import javax.swing.SwingConstants;

import smack.comm.SBXPCProxy;
import smack.comm.output.OneStringOutput;
import smack.comm.sample.global.SysUtil;

public class EnrollCustom1Frame extends JFrame {
	private JLabel lblMessage;
	private JLabel lblEnrollNumber;
	private JLabel lblUserMessage;
	private JTextField txtEnrollNumber;
	private JTextField txtHolidays;
	private JButton btnGetUserMessage;
	private JButton btnSetUserMessage;
	private JTextArea txtUserMessage;
	private JTextField txtVerifyCount;

	/**
	 * Launch the application.
	 */
	public static void main(String[] args) {
		EventQueue.invokeLater(new Runnable() {
			public void run() {
				try {
					EnrollCustom1Frame frame = new EnrollCustom1Frame();
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
	public EnrollCustom1Frame() {
		addWindowListener(new WindowAdapter() {
			@Override
			public void windowClosing(WindowEvent arg0) {
				if (MainFrame.getInstance() != null)
					MainFrame.getInstance().setVisible(true);
			}
		});
	
		setTitle("EnrollCustom1");
		setBounds(100, 100, 630, 412);
		setDefaultCloseOperation(javax.swing.WindowConstants.DISPOSE_ON_CLOSE);
		getContentPane().setLayout(null);
		
		lblMessage = new JLabel("Message");
		lblMessage.setHorizontalAlignment(SwingConstants.CENTER);
		lblMessage.setFont(new Font("Segoe UI", Font.BOLD, 18));
		lblMessage.setBorder(javax.swing.BorderFactory.createBevelBorder(javax.swing.border.BevelBorder.LOWERED));
		lblMessage.setBounds(10, 11, 412, 40);
		getContentPane().add(lblMessage);
		
		lblEnrollNumber = new JLabel("Enroll Number:");
		lblEnrollNumber.setHorizontalAlignment(SwingConstants.RIGHT);
		lblEnrollNumber.setBounds(10, 64, 115, 14);
		getContentPane().add(lblEnrollNumber);
		
		lblUserMessage = new JLabel("Message:");
		lblUserMessage.setHorizontalAlignment(SwingConstants.RIGHT);
		lblUserMessage.setBounds(10, 95, 115, 14);
		getContentPane().add(lblUserMessage);
		
		txtEnrollNumber = new JTextField();
		txtEnrollNumber.setBounds(135, 64, 227, 20);
		getContentPane().add(txtEnrollNumber);
		txtEnrollNumber.setColumns(10);
		txtEnrollNumber.setText("1");
		
		txtHolidays = new JTextField();
		txtHolidays.setBounds(135, 256, 227, 20);
		getContentPane().add(txtHolidays);
		txtHolidays.setColumns(10);
	
		btnGetUserMessage = new JButton("Get");
		btnGetUserMessage.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				long userId;
				try {
					userId = Long.parseLong(txtEnrollNumber.getText());
				} catch (NumberFormatException ne) {
					JOptionPane.showMessageDialog(null, "Invalid Enroll Number");
					return;
				}

				OneStringOutput output;
				output = SysUtil.MakeXMLCommandHeader("GetUserMessage");
				output = SBXPCProxy.XML_AddLong(output.value, "UserID", userId);

				output = SBXPCProxy.GeneralOperationXML(SysUtil.MachineNumber, output.value);
				if (output.isSuccess()) {
					txtUserMessage.setText("");

					String base64_name = SBXPCProxy.XML_ParseString(output.value, "Message").value;
					if (base64_name != null)
					{
						try 
						{
							byte[] name_binary = Base64.getDecoder().decode(base64_name);
							int index = 0;
                            for (int i = 0; i < name_binary.length - 1; i += 2)
                            {
                                if (name_binary[i] == 0 && name_binary[i + 1] == 0)
                                {
                                    index = i;
                                    break;
                                }
                            }
							char[] char_binary = new char[index / 2];
							for (int i = 0; i < index / 2; i++)
							{
								int hi = name_binary[i * 2 + 1];
								int lo = name_binary[i * 2];
								if (hi < 0) hi += 256;
								if (lo < 0) lo += 256;
								
								char_binary[i] = (char)(lo + hi * 256);
							}
							
							txtUserMessage.setText(String.valueOf(char_binary, 0, index / 2));
						}
						catch(Exception ex)
						{
						}
					}

					lblMessage.setText("Success!");
				} else {
					int errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;
					lblMessage.setText(SysUtil.ErrorPrint(errorCode));
				}	
			}
		});
		btnGetUserMessage.setBounds(450, 91, 66, 28);
		getContentPane().add(btnGetUserMessage);
		
		btnSetUserMessage = new JButton("Set");
		btnSetUserMessage.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				long userId;
				try {
					userId = Long.parseLong(txtEnrollNumber.getText());
				} catch (NumberFormatException ne) {
					JOptionPane.showMessageDialog(null, "Invalid Enroll Number");
					return;
				}

				lblMessage.setText(SysUtil.WORKING);
				invalidate();

				int errorCode;
				OneStringOutput output;
				output = SysUtil.MakeXMLCommandHeader("SetUserMessage");
				output = SBXPCProxy.XML_AddLong(output.value, "UserID", userId);
				{
					char[] chs = txtUserMessage.getText().toCharArray();
					byte[] bys = new byte[chs.length * 2];
					for (int i = 0; i < chs.length; i++)
					{
						bys[i * 2 + 1] = (byte)(chs[i] / 256);
						bys[i * 2] = (byte)(chs[i] & 0xFF);
					}
					output = SBXPCProxy.XML_AddString(output.value, "Message", Base64.getEncoder().encodeToString(bys));
				}
				output = SBXPCProxy.GeneralOperationXML(SysUtil.MachineNumber, output.value);

				if (output.isSuccess()) {
					lblMessage.setText("Success!");
				} else {
					errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;
					lblMessage.setText(SysUtil.ErrorPrint(errorCode));
				}
			}
		});
		btnSetUserMessage.setBounds(526, 91, 66, 28);
		getContentPane().add(btnSetUserMessage);
		
		txtUserMessage = new JTextArea();
		txtUserMessage.setBounds(135, 90, 287, 113);
		getContentPane().add(txtUserMessage);
		
		JLabel lblBalanceTime = new JLabel("Balance Time:");
		lblBalanceTime.setHorizontalAlignment(SwingConstants.RIGHT);
		lblBalanceTime.setBounds(10, 225, 115, 14);
		getContentPane().add(lblBalanceTime);
		
		JLabel lblHolidays = new JLabel("Holidays:");
		lblHolidays.setHorizontalAlignment(SwingConstants.RIGHT);
		lblHolidays.setBounds(10, 259, 115, 14);
		getContentPane().add(lblHolidays);
		
		JTextField dtBalanceTime_HH = new JFormattedTextField();
		dtBalanceTime_HH.setText("0");
		dtBalanceTime_HH.setBounds(135, 222, 35, 20);
		getContentPane().add(dtBalanceTime_HH);
		
		JLabel label = new JLabel(":");
		label.setHorizontalAlignment(SwingConstants.CENTER);
		label.setBounds(169, 225, 15, 14);
		getContentPane().add(label);
		
		JTextField dtBalanceTime_MM = new JFormattedTextField();
		dtBalanceTime_MM.setText("0");
		dtBalanceTime_MM.setBounds(184, 222, 35, 20);
		getContentPane().add(dtBalanceTime_MM);

		JCheckBox chkUseVerifyCount = new JCheckBox("Use Verify Count");
		chkUseVerifyCount.setBounds(135, 300, 227, 23);
		getContentPane().add(chkUseVerifyCount);
		
		JLabel lblVerifycount = new JLabel("VerifyCount(0~255):");
		lblVerifycount.setHorizontalAlignment(SwingConstants.RIGHT);
		lblVerifycount.setBounds(10, 330, 115, 14);
		getContentPane().add(lblVerifycount);
		
		txtVerifyCount = new JTextField();
		txtVerifyCount.setText("1");
		txtVerifyCount.setColumns(10);
		txtVerifyCount.setBounds(135, 327, 227, 20);
		getContentPane().add(txtVerifyCount);
		
		JButton btnGetUserBalanceTime = new JButton("Get");
		btnGetUserBalanceTime.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				long userId;
				try {
					userId = Long.parseLong(txtEnrollNumber.getText());
				} catch (NumberFormatException ne) {
					JOptionPane.showMessageDialog(null, "Invalid Enroll Number");
					return;
				}

				OneStringOutput output;
				output = SysUtil.MakeXMLCommandHeader("GetUserBalanceTime");
				output = SBXPCProxy.XML_AddLong(output.value, "UserID", userId);

				output = SBXPCProxy.GeneralOperationXML(SysUtil.MachineNumber, output.value);
				if (output.isSuccess()) {
					int lBalanceTimeInMinues = SBXPCProxy.XML_ParseInt(output.value, "BalanceTimeInMinues");
					dtBalanceTime_HH.setText(String.valueOf(lBalanceTimeInMinues / 60));
					dtBalanceTime_MM.setText(String.valueOf(lBalanceTimeInMinues % 60));

					lblMessage.setText("Success!");
				} else {
					int errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;
					lblMessage.setText(SysUtil.ErrorPrint(errorCode));
				}
			}
		});
		btnGetUserBalanceTime.setBounds(450, 212, 66, 28);
		getContentPane().add(btnGetUserBalanceTime);
		
		JButton btnSetUserBalanceTime = new JButton("Set");
		btnSetUserBalanceTime.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				long userId;
				try {
					userId = Long.parseLong(txtEnrollNumber.getText());
				} catch (NumberFormatException ne) {
					JOptionPane.showMessageDialog(null, "Invalid Enroll Number");
					return;
				}

				lblMessage.setText(SysUtil.WORKING);
				invalidate();

				int errorCode;
				OneStringOutput output;
				output = SysUtil.MakeXMLCommandHeader("SetUserBalanceTime");
				output = SBXPCProxy.XML_AddLong(output.value, "UserID", userId);

				int lBalanceTimeInMinues = Integer.parseInt(dtBalanceTime_HH.getText()) * 60 + Integer.parseInt(dtBalanceTime_MM.getText());
				output = SBXPCProxy.XML_AddLong(output.value, "BalanceTimeInMinues", lBalanceTimeInMinues);

				output = SBXPCProxy.GeneralOperationXML(SysUtil.MachineNumber, output.value);

				if (output.isSuccess()) {
					lblMessage.setText("Success!");
				} else {
					errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;
					lblMessage.setText(SysUtil.ErrorPrint(errorCode));
				}
			}
		});
		btnSetUserBalanceTime.setBounds(526, 212, 66, 28);
		getContentPane().add(btnSetUserBalanceTime);
		
		JButton btnGetUserHolidays = new JButton("Get");
		btnGetUserHolidays.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				long userId;
				try {
					userId = Long.parseLong(txtEnrollNumber.getText());
				} catch (NumberFormatException ne) {
					JOptionPane.showMessageDialog(null, "Invalid Enroll Number");
					return;
				}

				OneStringOutput output;
				output = SysUtil.MakeXMLCommandHeader("GetUserHolidays");
				output = SBXPCProxy.XML_AddLong(output.value, "UserID", userId);

				output = SBXPCProxy.GeneralOperationXML(SysUtil.MachineNumber, output.value);
				if (output.isSuccess()) {
					int lHolidaysInDays10 = SBXPCProxy.XML_ParseInt(output.value, "HolidaysInDays10");
					txtHolidays.setText((lHolidaysInDays10 / 10) + "." + (lHolidaysInDays10 % 10));

					lblMessage.setText("Success!");
				} else {
					int errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;
					lblMessage.setText(SysUtil.ErrorPrint(errorCode));
				}
			}
		});
		btnGetUserHolidays.setBounds(450, 248, 66, 28);
		getContentPane().add(btnGetUserHolidays);
		
		JButton btnSetUserHolidays = new JButton("Set");
		btnSetUserHolidays.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				long userId;
				try {
					userId = Long.parseLong(txtEnrollNumber.getText());
				} catch (NumberFormatException ne) {
					JOptionPane.showMessageDialog(null, "Invalid Enroll Number");
					return;
				}

				lblMessage.setText(SysUtil.WORKING);
				invalidate();

				int errorCode;
				OneStringOutput output;
				output = SysUtil.MakeXMLCommandHeader("SetUserHolidays");
				output = SBXPCProxy.XML_AddLong(output.value, "UserID", userId);

				int lHolidaysInDays10 = (int)(Double.parseDouble(txtHolidays.getText()) * 10);
				txtHolidays.setText((lHolidaysInDays10 / 10) + "." + (lHolidaysInDays10 % 10));
				
				output = SBXPCProxy.XML_AddLong(output.value, "HolidaysInDays10", lHolidaysInDays10);

				output = SBXPCProxy.GeneralOperationXML(SysUtil.MachineNumber, output.value);

				if (output.isSuccess()) {
					lblMessage.setText("Success!");
				} else {
					errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;
					lblMessage.setText(SysUtil.ErrorPrint(errorCode));
				}
			}
		});
		btnSetUserHolidays.setBounds(526, 248, 66, 28);
		getContentPane().add(btnSetUserHolidays);
		
		JButton btnGetUserVerifyCount = new JButton("Get");
		btnGetUserVerifyCount.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				long userId;
				try {
					userId = Long.parseLong(txtEnrollNumber.getText());
				} catch (NumberFormatException ne) {
					JOptionPane.showMessageDialog(null, "Invalid Enroll Number");
					return;
				}

				OneStringOutput output;
				output = SysUtil.MakeXMLCommandHeader("GetUserVerifyCount");
				output = SBXPCProxy.XML_AddLong(output.value, "UserID", userId);

				output = SBXPCProxy.GeneralOperationXML(SysUtil.MachineNumber, output.value);
				if (output.isSuccess()) {
					chkUseVerifyCount.setSelected(SBXPCProxy.XML_ParseInt(output.value, "Used") != 0);
					txtVerifyCount.setText(String.valueOf(SBXPCProxy.XML_ParseInt(output.value, "Count")));

					lblMessage.setText("Success!");
				} else {
					int errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;
					lblMessage.setText(SysUtil.ErrorPrint(errorCode));
				}
			}
		});
		btnGetUserVerifyCount.setBounds(450, 319, 66, 28);
		getContentPane().add(btnGetUserVerifyCount);
		
		JButton btnSetUserVerifyCount = new JButton("Set");
		btnSetUserVerifyCount.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				long userId;
				try {
					userId = Long.parseLong(txtEnrollNumber.getText());
				} catch (NumberFormatException ne) {
					JOptionPane.showMessageDialog(null, "Invalid Enroll Number");
					return;
				}

				lblMessage.setText(SysUtil.WORKING);
				invalidate();

				int errorCode;
				OneStringOutput output;
				output = SysUtil.MakeXMLCommandHeader("SetUserVerifyCount");
				output = SBXPCProxy.XML_AddLong(output.value, "UserID", userId);

				output = SBXPCProxy.XML_AddInt(output.value, "Used", chkUseVerifyCount.isSelected() ? 1 : 0);
				output = SBXPCProxy.XML_AddInt(output.value, "Count", Integer.parseInt(txtVerifyCount.getText()));

				output = SBXPCProxy.GeneralOperationXML(SysUtil.MachineNumber, output.value);

				if (output.isSuccess()) {
					lblMessage.setText("Success!");
				} else {
					errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;
					lblMessage.setText(SysUtil.ErrorPrint(errorCode));
				}
			}
		});
		btnSetUserVerifyCount.setBounds(526, 319, 66, 28);
		getContentPane().add(btnSetUserVerifyCount);
	}
}
