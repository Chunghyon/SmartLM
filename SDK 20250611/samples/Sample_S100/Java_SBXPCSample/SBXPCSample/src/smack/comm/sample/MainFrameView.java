/*
 * MainFrameView.java
 */

package smack.comm.sample;

import java.util.EventObject;
import org.jdesktop.application.Action;
import org.jdesktop.application.ResourceMap;
import org.jdesktop.application.SingleFrameApplication;
import org.jdesktop.application.FrameView;
import org.jdesktop.application.TaskMonitor;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import javax.swing.Timer;
import javax.swing.Icon;
import javax.swing.JDialog;
import javax.swing.JFrame;
import org.jdesktop.application.Application;
import smack.comm.SBXPCProxy;
import smack.comm.sample.global.SysUtil;

/**
 * The application's main frame.
 */

public class MainFrameView extends FrameView implements Application.ExitListener{
    
    @SuppressWarnings("LeakingThisInConstructor")
    public MainFrameView(SingleFrameApplication app) {
        super(app);
        
        app.addExitListener(this);
        
        initComponents();

        // status bar initialization - message timeout, idle icon and busy animation, etc
        ResourceMap resourceMap = getResourceMap();
        int messageTimeout = resourceMap.getInteger("StatusBar.messageTimeout");
        messageTimer = new Timer(messageTimeout, new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                statusMessageLabel.setText("");
            }
        });
        messageTimer.setRepeats(false);
        int busyAnimationRate = resourceMap.getInteger("StatusBar.busyAnimationRate");
        for (int i = 0; i < busyIcons.length; i++) {
            busyIcons[i] = resourceMap.getIcon("StatusBar.busyIcons[" + i + "]");
        }
        busyIconTimer = new Timer(busyAnimationRate, new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                busyIconIndex = (busyIconIndex + 1) % busyIcons.length;
                statusAnimationLabel.setIcon(busyIcons[busyIconIndex]);
            }
        });
        idleIcon = resourceMap.getIcon("StatusBar.idleIcon");
        statusAnimationLabel.setIcon(idleIcon);
        progressBar.setVisible(false);

        // connecting action tasks to status bar via TaskMonitor
        TaskMonitor taskMonitor = new TaskMonitor(getApplication().getContext());
        taskMonitor.addPropertyChangeListener(new java.beans.PropertyChangeListener() {
            public void propertyChange(java.beans.PropertyChangeEvent evt) {
                String propertyName = evt.getPropertyName();
                if ("started".equals(propertyName)) {
                    if (!busyIconTimer.isRunning()) {
                        statusAnimationLabel.setIcon(busyIcons[0]);
                        busyIconIndex = 0;
                        busyIconTimer.start();
                    }
                    progressBar.setVisible(true);
                    progressBar.setIndeterminate(true);
                } else if ("done".equals(propertyName)) {
                    busyIconTimer.stop();
                    statusAnimationLabel.setIcon(idleIcon);
                    progressBar.setVisible(false);
                    progressBar.setValue(0);
                } else if ("message".equals(propertyName)) {
                    String text = (String)(evt.getNewValue());
                    statusMessageLabel.setText((text == null) ? "" : text);
                    messageTimer.restart();
                } else if ("progress".equals(propertyName)) {
                    int value = (Integer)(evt.getNewValue());
                    progressBar.setVisible(true);
                    progressBar.setIndeterminate(false);
                    progressBar.setValue(value);
                }
            }
        });
    }

    @Action
    public void showAboutBox() {
        if (aboutBox == null) {
            JFrame mainFrame = MainFrame.getApplication().getMainFrame();
            aboutBox = new MainFrameAboutBox(mainFrame);
            aboutBox.setLocationRelativeTo(mainFrame);
        }
        MainFrame.getApplication().show(aboutBox);
    }
    
    private void enableManagementGroup(boolean bEnable)
    {
        btnEnroll.setEnabled(bEnable);
        btnLog.setEnabled(bEnable);
        btnSysInfo.setEnabled(bEnable);
        btnLockControl.setEnabled(bEnable);
        btnBellTime.setEnabled(bEnable);
        btnTrMode.setEnabled(bEnable);
        btnProductCode.setEnabled(bEnable);
        btnAccessTz.setEnabled(bEnable);
        btnModeTZone.setEnabled(bEnable);
        btnHoliday.setEnabled(bEnable);
        
        btnClose.setEnabled(bEnable);
        btnOpen.setEnabled(!bEnable);
        
        optSerial.setEnabled(!bEnable);
        optNetwork.setEnabled(!bEnable);
        
        switchConnectMode(optNetwork.isSelected());
    }
    
    private void switchConnectMode(boolean bNetwork)
    {
        cmbCommPort.setEnabled(!bNetwork);
        cmbBaudrate.setEnabled(!bNetwork);
        txtIPAddress.setEnabled(bNetwork);
        txtPortNumber.setEnabled(bNetwork);
        txtPassword.setEnabled(bNetwork);
    }

    /** This method is called from within the constructor to
     * initialize the form.
     * WARNING: Do NOT modify this code. The content of this method is
     * always regenerated by the Form Editor.
     */
    @SuppressWarnings("unchecked")
    // <editor-fold defaultstate="collapsed" desc="Generated Code">//GEN-BEGIN:initComponents
    private void initComponents() {

        mainPanel = new javax.swing.JPanel();
        jPanel1 = new javax.swing.JPanel();
        jLabel1 = new javax.swing.JLabel();
        cmbMachineNumber = new javax.swing.JComboBox();
        btnOpen = new javax.swing.JButton();
        btnClose = new javax.swing.JButton();
        jLabel3 = new javax.swing.JLabel();
        jLabel4 = new javax.swing.JLabel();
        jPanel3 = new javax.swing.JPanel();
        jLabel2 = new javax.swing.JLabel();
        jLabel5 = new javax.swing.JLabel();
        jLabel6 = new javax.swing.JLabel();
        txtPassword = new javax.swing.JTextField();
        txtIPAddress = new javax.swing.JTextField();
        txtPortNumber = new javax.swing.JTextField();
        optNetwork = new javax.swing.JRadioButton();
        jPanel4 = new javax.swing.JPanel();
        btnExit = new javax.swing.JButton();
        btnLockControl = new javax.swing.JButton();
        btnLog = new javax.swing.JButton();
        btnEnroll = new javax.swing.JButton();
        btnSysInfo = new javax.swing.JButton();
        btnBellTime = new javax.swing.JButton();
        btnTrMode = new javax.swing.JButton();
        btnProductCode = new javax.swing.JButton();
        btnAccessTz = new javax.swing.JButton();
        btnHoliday = new javax.swing.JButton();
        btnModeTZone = new javax.swing.JButton();
        jPanel5 = new javax.swing.JPanel();
        cmbCommPort = new javax.swing.JComboBox();
        jLabel9 = new javax.swing.JLabel();
        cmbBaudrate = new javax.swing.JComboBox();
        jLabel10 = new javax.swing.JLabel();
        optSerial = new javax.swing.JRadioButton();
        menuBar = new javax.swing.JMenuBar();
        javax.swing.JMenu fileMenu = new javax.swing.JMenu();
        javax.swing.JMenuItem exitMenuItem = new javax.swing.JMenuItem();
        javax.swing.JMenu helpMenu = new javax.swing.JMenu();
        javax.swing.JMenuItem aboutMenuItem = new javax.swing.JMenuItem();
        statusPanel = new javax.swing.JPanel();
        javax.swing.JSeparator statusPanelSeparator = new javax.swing.JSeparator();
        statusMessageLabel = new javax.swing.JLabel();
        statusAnimationLabel = new javax.swing.JLabel();
        progressBar = new javax.swing.JProgressBar();
        buttonGroup1 = new javax.swing.ButtonGroup();

        mainPanel.setName("mainPanel"); // NOI18N
        mainPanel.setLayout(new org.netbeans.lib.awtextra.AbsoluteLayout());

        org.jdesktop.application.ResourceMap resourceMap = org.jdesktop.application.Application.getInstance(smack.comm.sample.MainFrame.class).getContext().getResourceMap(MainFrameView.class);
        jPanel1.setBorder(javax.swing.BorderFactory.createTitledBorder(null, resourceMap.getString("connectPanel.border.title"), javax.swing.border.TitledBorder.DEFAULT_JUSTIFICATION, javax.swing.border.TitledBorder.DEFAULT_POSITION, resourceMap.getFont("connectPanel.border.titleFont"), resourceMap.getColor("connectPanel.border.titleColor"))); // NOI18N
        jPanel1.setToolTipText(resourceMap.getString("connectPanel.toolTipText")); // NOI18N
        jPanel1.setName("connectPanel"); // NOI18N
        jPanel1.setLayout(new org.netbeans.lib.awtextra.AbsoluteLayout());

        jLabel1.setFont(resourceMap.getFont("jLabel1.font")); // NOI18N
        jLabel1.setText(resourceMap.getString("jLabel1.text")); // NOI18N
        jLabel1.setName("jLabel1"); // NOI18N
        jPanel1.add(jLabel1, new org.netbeans.lib.awtextra.AbsoluteConstraints(20, 30, -1, -1));

        cmbMachineNumber.setFont(resourceMap.getFont("cmbMachineNumber.font")); // NOI18N
        cmbMachineNumber.setModel(new javax.swing.DefaultComboBoxModel(new String[] { "1", "2", "3", "4", "5", "6", "7", "8", "" }));
        cmbMachineNumber.setName("cmbMachineNumber"); // NOI18N
        cmbMachineNumber.addMouseListener(new java.awt.event.MouseAdapter() {
            public void mouseClicked(java.awt.event.MouseEvent evt) {
                cmbMachineNumberMouseClicked(evt);
            }
        });
        jPanel1.add(cmbMachineNumber, new org.netbeans.lib.awtextra.AbsoluteConstraints(130, 30, 80, -1));

        btnOpen.setFont(resourceMap.getFont("btnOpen.font")); // NOI18N
        btnOpen.setText(resourceMap.getString("btnOpen.text")); // NOI18N
        btnOpen.setName("btnOpen"); // NOI18N
        btnOpen.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                btnOpenActionPerformed(evt);
            }
        });
        jPanel1.add(btnOpen, new org.netbeans.lib.awtextra.AbsoluteConstraints(340, 20, 90, 30));

        btnClose.setFont(resourceMap.getFont("btnClose.font")); // NOI18N
        btnClose.setText(resourceMap.getString("btnClose.text")); // NOI18N
        btnClose.setEnabled(false);
        btnClose.setName("btnClose"); // NOI18N
        btnClose.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                btnCloseActionPerformed(evt);
            }
        });
        jPanel1.add(btnClose, new org.netbeans.lib.awtextra.AbsoluteConstraints(440, 20, 90, 30));

        mainPanel.add(jPanel1, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 70, 550, 70));

        jLabel3.setFont(resourceMap.getFont("jLabel3.font")); // NOI18N
        jLabel3.setForeground(resourceMap.getColor("jLabel3.foreground")); // NOI18N
        jLabel3.setText(resourceMap.getString("jLabel3.text")); // NOI18N
        jLabel3.setName("jLabel3"); // NOI18N
        mainPanel.add(jLabel3, new org.netbeans.lib.awtextra.AbsoluteConstraints(190, 10, -1, -1));

        jLabel4.setFont(resourceMap.getFont("jLabel4.font")); // NOI18N
        jLabel4.setForeground(resourceMap.getColor("jLabel4.foreground")); // NOI18N
        jLabel4.setText(resourceMap.getString("jLabel4.text")); // NOI18N
        jLabel4.setName("jLabel4"); // NOI18N
        mainPanel.add(jLabel4, new org.netbeans.lib.awtextra.AbsoluteConstraints(240, 40, -1, -1));

        jPanel3.setBorder(javax.swing.BorderFactory.createTitledBorder(null, resourceMap.getString("NetworkPanel.border.title"), javax.swing.border.TitledBorder.DEFAULT_JUSTIFICATION, javax.swing.border.TitledBorder.DEFAULT_POSITION, resourceMap.getFont("NetworkPanel.border.titleFont"), resourceMap.getColor("NetworkPanel.border.titleColor"))); // NOI18N
        jPanel3.setToolTipText(resourceMap.getString("NetworkPanel.toolTipText")); // NOI18N
        jPanel3.setName("NetworkPanel"); // NOI18N
        jPanel3.setLayout(new org.netbeans.lib.awtextra.AbsoluteLayout());

        jLabel2.setFont(resourceMap.getFont("jLabel2.font")); // NOI18N
        jLabel2.setText(resourceMap.getString("jLabel2.text")); // NOI18N
        jLabel2.setName("jLabel2"); // NOI18N
        jPanel3.add(jLabel2, new org.netbeans.lib.awtextra.AbsoluteConstraints(20, 90, -1, -1));

        jLabel5.setFont(resourceMap.getFont("jLabel5.font")); // NOI18N
        jLabel5.setText(resourceMap.getString("jLabel5.text")); // NOI18N
        jLabel5.setName("jLabel5"); // NOI18N
        jPanel3.add(jLabel5, new org.netbeans.lib.awtextra.AbsoluteConstraints(20, 30, -1, -1));

        jLabel6.setFont(resourceMap.getFont("jLabel6.font")); // NOI18N
        jLabel6.setText(resourceMap.getString("jLabel6.text")); // NOI18N
        jLabel6.setName("jLabel6"); // NOI18N
        jPanel3.add(jLabel6, new org.netbeans.lib.awtextra.AbsoluteConstraints(20, 60, -1, -1));

        txtPassword.setFont(resourceMap.getFont("txtPassword.font")); // NOI18N
        txtPassword.setText(resourceMap.getString("txtPassword.text")); // NOI18N
        txtPassword.setName("txtPassword"); // NOI18N
        jPanel3.add(txtPassword, new org.netbeans.lib.awtextra.AbsoluteConstraints(100, 90, 150, -1));

        txtIPAddress.setFont(resourceMap.getFont("txtIPAddress.font")); // NOI18N
        txtIPAddress.setText(resourceMap.getString("txtIPAddress.text")); // NOI18N
        txtIPAddress.setName("txtIPAddress"); // NOI18N
        jPanel3.add(txtIPAddress, new org.netbeans.lib.awtextra.AbsoluteConstraints(100, 30, 150, -1));

        txtPortNumber.setFont(resourceMap.getFont("txtPortNumber.font")); // NOI18N
        txtPortNumber.setText(resourceMap.getString("txtPortNumber.text")); // NOI18N
        txtPortNumber.setName("txtPortNumber"); // NOI18N
        jPanel3.add(txtPortNumber, new org.netbeans.lib.awtextra.AbsoluteConstraints(100, 60, 150, -1));

        buttonGroup1.add(optNetwork);
        optNetwork.setFont(resourceMap.getFont("optNetwork.font")); // NOI18N
        optNetwork.setSelected(true);
        optNetwork.setText(resourceMap.getString("optNetwork.text")); // NOI18N
        optNetwork.setName("optNetwork"); // NOI18N
        optNetwork.addChangeListener(new javax.swing.event.ChangeListener() {
            public void stateChanged(javax.swing.event.ChangeEvent evt) {
                optNetworkStateChanged(evt);
            }
        });
        jPanel3.add(optNetwork, new org.netbeans.lib.awtextra.AbsoluteConstraints(8, 0, -1, -1));

        mainPanel.add(jPanel3, new org.netbeans.lib.awtextra.AbsoluteConstraints(290, 140, 270, 130));

        jPanel4.setBorder(javax.swing.BorderFactory.createTitledBorder(null, resourceMap.getString("ManagementPanel.border.title"), javax.swing.border.TitledBorder.DEFAULT_JUSTIFICATION, javax.swing.border.TitledBorder.DEFAULT_POSITION, resourceMap.getFont("ManagementPanel.border.titleFont"), resourceMap.getColor("ManagementPanel.border.titleColor"))); // NOI18N
        jPanel4.setToolTipText(resourceMap.getString("ManagementPanel.toolTipText")); // NOI18N
        jPanel4.setName("ManagementPanel"); // NOI18N
        jPanel4.setLayout(new org.netbeans.lib.awtextra.AbsoluteLayout());

        btnExit.setFont(resourceMap.getFont("btnExit.font")); // NOI18N
        btnExit.setText(resourceMap.getString("btnExit.text")); // NOI18N
        btnExit.setName("btnExit"); // NOI18N
        btnExit.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                btnExitActionPerformed(evt);
            }
        });
        jPanel4.add(btnExit, new org.netbeans.lib.awtextra.AbsoluteConstraints(20, 120, 210, 30));

        btnLockControl.setFont(resourceMap.getFont("btnLockControl.font")); // NOI18N
        btnLockControl.setText(resourceMap.getString("btnLockControl.text")); // NOI18N
        btnLockControl.setEnabled(false);
        btnLockControl.setName("btnLockControl"); // NOI18N
        btnLockControl.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                btnLockControlActionPerformed(evt);
            }
        });
        jPanel4.add(btnLockControl, new org.netbeans.lib.awtextra.AbsoluteConstraints(400, 30, 130, 30));

        btnLog.setFont(resourceMap.getFont("btnLog.font")); // NOI18N
        btnLog.setText(resourceMap.getString("btnLog.text")); // NOI18N
        btnLog.setEnabled(false);
        btnLog.setName("btnLog"); // NOI18N
        btnLog.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                btnLogActionPerformed(evt);
            }
        });
        jPanel4.add(btnLog, new org.netbeans.lib.awtextra.AbsoluteConstraints(20, 60, 210, 30));

        btnEnroll.setFont(resourceMap.getFont("btnEnroll.font")); // NOI18N
        btnEnroll.setText(resourceMap.getString("btnEnroll.text")); // NOI18N
        btnEnroll.setEnabled(false);
        btnEnroll.setName("btnEnroll"); // NOI18N
        btnEnroll.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                btnEnrollActionPerformed(evt);
            }
        });
        jPanel4.add(btnEnroll, new org.netbeans.lib.awtextra.AbsoluteConstraints(20, 30, 210, 30));

        btnSysInfo.setFont(resourceMap.getFont("btnSysInfo.font")); // NOI18N
        btnSysInfo.setText(resourceMap.getString("btnSysInfo.text")); // NOI18N
        btnSysInfo.setEnabled(false);
        btnSysInfo.setName("btnSysInfo"); // NOI18N
        btnSysInfo.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                btnSysInfoActionPerformed(evt);
            }
        });
        jPanel4.add(btnSysInfo, new org.netbeans.lib.awtextra.AbsoluteConstraints(270, 30, 130, 30));

        btnBellTime.setFont(resourceMap.getFont("btnBellTime.font")); // NOI18N
        btnBellTime.setText(resourceMap.getString("btnBellTime.text")); // NOI18N
        btnBellTime.setEnabled(false);
        btnBellTime.setName("btnBellTime"); // NOI18N
        btnBellTime.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                btnBellTimeActionPerformed(evt);
            }
        });
        jPanel4.add(btnBellTime, new org.netbeans.lib.awtextra.AbsoluteConstraints(270, 60, 130, 30));

        btnTrMode.setFont(resourceMap.getFont("btnTrMode.font")); // NOI18N
        btnTrMode.setText(resourceMap.getString("btnTrMode.text")); // NOI18N
        btnTrMode.setEnabled(false);
        btnTrMode.setName("btnTrMode"); // NOI18N
        btnTrMode.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                btnTrModeActionPerformed(evt);
            }
        });
        jPanel4.add(btnTrMode, new org.netbeans.lib.awtextra.AbsoluteConstraints(400, 60, 130, 30));

        btnProductCode.setFont(resourceMap.getFont("btnProductCode.font")); // NOI18N
        btnProductCode.setText(resourceMap.getString("btnProductCode.text")); // NOI18N
        btnProductCode.setEnabled(false);
        btnProductCode.setName("btnProductCode"); // NOI18N
        btnProductCode.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                btnProductCodeActionPerformed(evt);
            }
        });
        jPanel4.add(btnProductCode, new org.netbeans.lib.awtextra.AbsoluteConstraints(270, 90, 130, 30));

        btnAccessTz.setFont(resourceMap.getFont("btnAccessTz.font")); // NOI18N
        btnAccessTz.setText(resourceMap.getString("btnAccessTz.text")); // NOI18N
        btnAccessTz.setEnabled(false);
        btnAccessTz.setName("btnAccessTz"); // NOI18N
        btnAccessTz.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                btnAccessTzActionPerformed(evt);
            }
        });
        jPanel4.add(btnAccessTz, new org.netbeans.lib.awtextra.AbsoluteConstraints(400, 90, 130, 30));

        btnHoliday.setFont(resourceMap.getFont("btnHoliday.font")); // NOI18N
        btnHoliday.setText(resourceMap.getString("btnHoliday.text")); // NOI18N
        btnHoliday.setEnabled(false);
        btnHoliday.setName("btnHoliday"); // NOI18N
        btnHoliday.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                btnHolidayActionPerformed(evt);
            }
        });
        jPanel4.add(btnHoliday, new org.netbeans.lib.awtextra.AbsoluteConstraints(400, 120, 130, 30));

        btnModeTZone.setFont(resourceMap.getFont("btnModeTZone.font")); // NOI18N
        btnModeTZone.setText(resourceMap.getString("btnModeTZone.text")); // NOI18N
        btnModeTZone.setEnabled(false);
        btnModeTZone.setName("btnModeTZone"); // NOI18N
        btnModeTZone.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                btnModeTZoneActionPerformed(evt);
            }
        });
        jPanel4.add(btnModeTZone, new org.netbeans.lib.awtextra.AbsoluteConstraints(270, 120, 130, 30));

        mainPanel.add(jPanel4, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 280, 550, 170));

        jPanel5.setBorder(javax.swing.BorderFactory.createTitledBorder(null, resourceMap.getString("SerialDevicePanel.border.title"), javax.swing.border.TitledBorder.DEFAULT_JUSTIFICATION, javax.swing.border.TitledBorder.DEFAULT_POSITION, resourceMap.getFont("SerialDevicePanel.border.titleFont"), resourceMap.getColor("SerialDevicePanel.border.titleColor"))); // NOI18N
        jPanel5.setToolTipText(resourceMap.getString("SerialDevicePanel.toolTipText")); // NOI18N
        jPanel5.setName("SerialDevicePanel"); // NOI18N
        jPanel5.setLayout(new org.netbeans.lib.awtextra.AbsoluteLayout());

        cmbCommPort.setFont(resourceMap.getFont("cmbCommPort.font")); // NOI18N
        cmbCommPort.setModel(new javax.swing.DefaultComboBoxModel(new String[] { "USB", "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", " " }));
        cmbCommPort.setEnabled(false);
        cmbCommPort.setName("cmbCommPort"); // NOI18N
        jPanel5.add(cmbCommPort, new org.netbeans.lib.awtextra.AbsoluteConstraints(120, 40, 80, 20));

        jLabel9.setFont(resourceMap.getFont("jLabel9.font")); // NOI18N
        jLabel9.setText(resourceMap.getString("jLabel9.text")); // NOI18N
        jLabel9.setName("jLabel9"); // NOI18N
        jPanel5.add(jLabel9, new org.netbeans.lib.awtextra.AbsoluteConstraints(30, 40, -1, 20));

        cmbBaudrate.setFont(resourceMap.getFont("cmbBaudrate.font")); // NOI18N
        cmbBaudrate.setModel(new javax.swing.DefaultComboBoxModel(new String[] { "9600", "19200", "38400", "57600", "115200", " " }));
        cmbBaudrate.setSelectedIndex(4);
        cmbBaudrate.setEnabled(false);
        cmbBaudrate.setName("cmbBaudrate"); // NOI18N
        jPanel5.add(cmbBaudrate, new org.netbeans.lib.awtextra.AbsoluteConstraints(120, 80, 80, 20));

        jLabel10.setFont(resourceMap.getFont("jLabel10.font")); // NOI18N
        jLabel10.setText(resourceMap.getString("jLabel10.text")); // NOI18N
        jLabel10.setName("jLabel10"); // NOI18N
        jPanel5.add(jLabel10, new org.netbeans.lib.awtextra.AbsoluteConstraints(30, 80, -1, 20));

        buttonGroup1.add(optSerial);
        optSerial.setFont(resourceMap.getFont("optNetwork.font")); // NOI18N
        optSerial.setText(resourceMap.getString("optSerial.text")); // NOI18N
        optSerial.setName("optSerial"); // NOI18N
        optSerial.addChangeListener(new javax.swing.event.ChangeListener() {
            public void stateChanged(javax.swing.event.ChangeEvent evt) {
                optSerialStateChanged(evt);
            }
        });
        jPanel5.add(optSerial, new org.netbeans.lib.awtextra.AbsoluteConstraints(8, 0, -1, -1));

        mainPanel.add(jPanel5, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 140, 270, 130));

        menuBar.setName("menuBar"); // NOI18N

        fileMenu.setText(resourceMap.getString("fileMenu.text")); // NOI18N
        fileMenu.setName("fileMenu"); // NOI18N

        javax.swing.ActionMap actionMap = org.jdesktop.application.Application.getInstance(smack.comm.sample.MainFrame.class).getContext().getActionMap(MainFrameView.class, this);
        exitMenuItem.setAction(actionMap.get("quit")); // NOI18N
        exitMenuItem.setName("exitMenuItem"); // NOI18N
        fileMenu.add(exitMenuItem);

        menuBar.add(fileMenu);

        helpMenu.setText(resourceMap.getString("helpMenu.text")); // NOI18N
        helpMenu.setName("helpMenu"); // NOI18N

        aboutMenuItem.setAction(actionMap.get("showAboutBox")); // NOI18N
        aboutMenuItem.setName("aboutMenuItem"); // NOI18N
        helpMenu.add(aboutMenuItem);

        menuBar.add(helpMenu);

        statusPanel.setName("statusPanel"); // NOI18N

        statusPanelSeparator.setName("statusPanelSeparator"); // NOI18N

        statusMessageLabel.setName("statusMessageLabel"); // NOI18N

        statusAnimationLabel.setHorizontalAlignment(javax.swing.SwingConstants.LEFT);
        statusAnimationLabel.setName("statusAnimationLabel"); // NOI18N

        progressBar.setName("progressBar"); // NOI18N

        javax.swing.GroupLayout statusPanelLayout = new javax.swing.GroupLayout(statusPanel);
        statusPanel.setLayout(statusPanelLayout);
        statusPanelLayout.setHorizontalGroup(
            statusPanelLayout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addComponent(statusPanelSeparator, javax.swing.GroupLayout.DEFAULT_SIZE, 576, Short.MAX_VALUE)
            .addGroup(statusPanelLayout.createSequentialGroup()
                .addContainerGap()
                .addComponent(statusMessageLabel)
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED, 402, Short.MAX_VALUE)
                .addComponent(progressBar, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addComponent(statusAnimationLabel)
                .addContainerGap())
        );
        statusPanelLayout.setVerticalGroup(
            statusPanelLayout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(statusPanelLayout.createSequentialGroup()
                .addComponent(statusPanelSeparator, javax.swing.GroupLayout.PREFERRED_SIZE, 2, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                .addGroup(statusPanelLayout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(statusMessageLabel)
                    .addComponent(statusAnimationLabel)
                    .addComponent(progressBar, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE))
                .addGap(3, 3, 3))
        );

        setComponent(mainPanel);
        setMenuBar(menuBar);
        setStatusBar(statusPanel);
    }// </editor-fold>//GEN-END:initComponents

    private void optSerialStateChanged(javax.swing.event.ChangeEvent evt) {//GEN-FIRST:event_optSerialStateChanged
        if(optSerial.isSelected())
            switchConnectMode(false);
    }//GEN-LAST:event_optSerialStateChanged

    private void optNetworkStateChanged(javax.swing.event.ChangeEvent evt) {//GEN-FIRST:event_optNetworkStateChanged
        if(optNetwork.isSelected())
            switchConnectMode(true);
    }//GEN-LAST:event_optNetworkStateChanged

    private void cmbMachineNumberMouseClicked(java.awt.event.MouseEvent evt) {//GEN-FIRST:event_cmbMachineNumberMouseClicked
        SysUtil.MachineNumber = cmbMachineNumber.getSelectedIndex() + 1;
    }//GEN-LAST:event_cmbMachineNumberMouseClicked

    private void btnOpenActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_btnOpenActionPerformed
        boolean ret = false;
        if(optNetwork.isSelected())
        {
            ret = SBXPCProxy.ConnectTcpip(  SysUtil.MachineNumber, 
                                            txtIPAddress.getText(), 
                                            Long.parseLong(txtPortNumber.getText()), 
                                            Long.parseLong(txtPassword.getText())
                                         );
        }else
        {
            long baudrate = Long.parseLong(cmbBaudrate.getSelectedItem().toString());
            ret = SBXPCProxy.ConnectSerial(SysUtil.MachineNumber, cmbCommPort.getSelectedIndex(), baudrate);
        }
        enableManagementGroup(ret);
    }//GEN-LAST:event_btnOpenActionPerformed

    private void btnCloseActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_btnCloseActionPerformed
        SBXPCProxy.Disconnect(SysUtil.MachineNumber);
        enableManagementGroup(false);
    }//GEN-LAST:event_btnCloseActionPerformed

    private void btnEnrollActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_btnEnrollActionPerformed
        EnrollMngFrame enroll_frame = new EnrollMngFrame();
        enroll_frame.setVisible(true);
        this.getFrame().setVisible(false);
    }//GEN-LAST:event_btnEnrollActionPerformed

    private void btnLogActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_btnLogActionPerformed
        LogMngFrame logMngFrame = new LogMngFrame();
        logMngFrame.setVisible(true);
        this.getFrame().setVisible(false);
    }//GEN-LAST:event_btnLogActionPerformed

    private void btnSysInfoActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_btnSysInfoActionPerformed
        SystemInfoFrame systemInfo_frame = new SystemInfoFrame();
        systemInfo_frame.setVisible(true);
        this.getFrame().setVisible(false);
    }//GEN-LAST:event_btnSysInfoActionPerformed

    private void btnLockControlActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_btnLockControlActionPerformed
        LockControlFrame lockControl_frame = new LockControlFrame();
        lockControl_frame.setVisible(true);
        this.getFrame().setVisible(false);
    }//GEN-LAST:event_btnLockControlActionPerformed

    private void btnBellTimeActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_btnBellTimeActionPerformed
        BellSettingFrame bellSetting_frame = new BellSettingFrame();
        bellSetting_frame.setVisible(true);
        this.getFrame().setVisible(false);
    }//GEN-LAST:event_btnBellTimeActionPerformed

    private void btnTrModeActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_btnTrModeActionPerformed
        TrModeFrame trMode_frame = new TrModeFrame();
        trMode_frame.setVisible(true);
        this.getFrame().setVisible(false);
    }//GEN-LAST:event_btnTrModeActionPerformed

    private void btnProductCodeActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_btnProductCodeActionPerformed
        ProductCodeFrame productCode_frame = new ProductCodeFrame();
        productCode_frame.setVisible(true);
        this.getFrame().setVisible(false);
    }//GEN-LAST:event_btnProductCodeActionPerformed

    private void btnAccessTzActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_btnAccessTzActionPerformed
        AccessTzFrame accessTz_frame = new AccessTzFrame();
        accessTz_frame.setVisible(true);
        this.getFrame().setVisible(false);
    }//GEN-LAST:event_btnAccessTzActionPerformed

    private void btnModeTZoneActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_btnModeTZoneActionPerformed
        TModeMngFrame tModeManage_frame = new TModeMngFrame();
        tModeManage_frame.setVisible(true);
        this.getFrame().setVisible(false);
    }//GEN-LAST:event_btnModeTZoneActionPerformed

    private void btnHolidayActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_btnHolidayActionPerformed
        HolidayFrame holiday_frame = new HolidayFrame();
        holiday_frame.setVisible(true);
        this.getFrame().setVisible(false);
    }//GEN-LAST:event_btnHolidayActionPerformed

    private void btnExitActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_btnExitActionPerformed
        Application.getInstance().exit();
    }//GEN-LAST:event_btnExitActionPerformed

    // Variables declaration - do not modify//GEN-BEGIN:variables
    private javax.swing.JButton btnAccessTz;
    private javax.swing.JButton btnBellTime;
    private javax.swing.JButton btnClose;
    private javax.swing.JButton btnEnroll;
    private javax.swing.JButton btnExit;
    private javax.swing.JButton btnHoliday;
    private javax.swing.JButton btnLockControl;
    private javax.swing.JButton btnLog;
    private javax.swing.JButton btnModeTZone;
    private javax.swing.JButton btnOpen;
    private javax.swing.JButton btnProductCode;
    private javax.swing.JButton btnSysInfo;
    private javax.swing.JButton btnTrMode;
    private javax.swing.ButtonGroup buttonGroup1;
    private javax.swing.JComboBox cmbBaudrate;
    private javax.swing.JComboBox cmbCommPort;
    private javax.swing.JComboBox cmbMachineNumber;
    private javax.swing.JLabel jLabel1;
    private javax.swing.JLabel jLabel10;
    private javax.swing.JLabel jLabel2;
    private javax.swing.JLabel jLabel3;
    private javax.swing.JLabel jLabel4;
    private javax.swing.JLabel jLabel5;
    private javax.swing.JLabel jLabel6;
    private javax.swing.JLabel jLabel9;
    private javax.swing.JPanel jPanel1;
    private javax.swing.JPanel jPanel3;
    private javax.swing.JPanel jPanel4;
    private javax.swing.JPanel jPanel5;
    private javax.swing.JPanel mainPanel;
    private javax.swing.JMenuBar menuBar;
    private javax.swing.JRadioButton optNetwork;
    private javax.swing.JRadioButton optSerial;
    private javax.swing.JProgressBar progressBar;
    private javax.swing.JLabel statusAnimationLabel;
    private javax.swing.JLabel statusMessageLabel;
    private javax.swing.JPanel statusPanel;
    private javax.swing.JTextField txtIPAddress;
    private javax.swing.JTextField txtPassword;
    private javax.swing.JTextField txtPortNumber;
    // End of variables declaration//GEN-END:variables

    private final Timer messageTimer;
    private final Timer busyIconTimer;
    private final Icon idleIcon;
    private final Icon[] busyIcons = new Icon[15];
    private int busyIconIndex = 0;

    private JDialog aboutBox;

    public boolean canExit(EventObject event) {
        return true;
    }

    public void willExit(EventObject event) {
        SBXPCProxy.Disconnect(SysUtil.MachineNumber);
    }
}
