/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

/*
 * AccessTzFrame.java
 *
 * Created on Aug 19, 2011, 1:19:07 PM
 */
package smack.comm.sample;

import java.text.DecimalFormat;
import java.text.NumberFormat;
import java.util.Calendar;
import java.util.Date;
import javax.swing.AbstractListModel;
import smack.comm.SBXPCProxy;
import smack.comm.output.GetDeviceLongInfoOutput;
import smack.comm.sample.global.SysUtil;

/**
 *
 * @author smackbio
 */
public class AccessTzFrame extends javax.swing.JFrame {
    
    public class DbTimeSection
    {
        public static final byte DEFAULT_VM = 5;
        
        private final int timeZoneIndex;
        private final int timeSectionIndex;
        
        public int startHour;
        public int startMinute;
        public int endHour;
        public int endMinute;
        
        public DbTimeSection(int zoneIndex, int sectionIndex)
        {
            timeZoneIndex = zoneIndex;
            timeSectionIndex = sectionIndex;
            
            startHour = 0;
            startMinute = 0;
            endHour = 23;
            endMinute = 59;
        }
        
        @Override
        public String toString()
        {
            String ret = "";
            NumberFormat formatter = new DecimalFormat("000");
            NumberFormat time_formatter = new DecimalFormat("00");
            ret += "[Tz] " + formatter.format(timeZoneIndex) + "-" + formatter.format(timeSectionIndex);
            ret += "[S] " + time_formatter.format(startHour) + ":" + time_formatter.format(startMinute) + " ";
            ret += "[E] " + time_formatter.format(endHour) + ":" + time_formatter.format(endMinute) + " ";
            return ret;
        }
    }
    
    class TimeZoneListModel extends AbstractListModel
    {
        public static final int TIME_ZONE_COUNT = 50;
        public static final int TIME_SECTION_COUNT = 8;
        public static final int ELEM_CNT = 4;
        public static final int TIME_ZONE_LIST_SIZE = TIME_ZONE_COUNT * TIME_SECTION_COUNT;
        public static final int VM_TZONE_COUNT = 10;

        private DbTimeSection[] timezoneInfoList;

        public TimeZoneListModel()
        {
            timezoneInfoList = new DbTimeSection[TIME_ZONE_LIST_SIZE];
            for(int i = 0; i < TIME_ZONE_COUNT; i ++)
            {
                for(int j = 0; j < TIME_SECTION_COUNT; j ++)
                    timezoneInfoList[i * TIME_SECTION_COUNT + j] = new DbTimeSection(i, j);
            }
        }
        
        public void setAllData(GetDeviceLongInfoOutput data)
        {
            int index; 
            if(data.value.length < TIME_ZONE_LIST_SIZE * ELEM_CNT)
                return;
            for(int i = 0; i < TIME_ZONE_COUNT; i ++)
            {
                for(int j = 0; j < TIME_SECTION_COUNT; j ++)
                {
                    index = i * TIME_SECTION_COUNT + j;
                    timezoneInfoList[index].startHour   = data.value[index * ELEM_CNT + 0];
                    timezoneInfoList[index].startMinute = data.value[index * ELEM_CNT + 1];
                    timezoneInfoList[index].endHour     = data.value[index * ELEM_CNT + 2];
                    timezoneInfoList[index].endMinute   = data.value[index * ELEM_CNT + 3];
                }
            }
            fireContentsChanged(this, 0, getSize());
        }
        
        public int[] getAllData()
        {
            int[] ret = new int[TIME_ZONE_LIST_SIZE * ELEM_CNT];
            int index;
            for(int i = 0; i < TIME_ZONE_COUNT; i ++)
            {
                for(int j = 0; j < TIME_SECTION_COUNT; j ++)
                {
                    index = i * TIME_SECTION_COUNT + j;
                    ret[index * ELEM_CNT + 0] = timezoneInfoList[index].startHour;
                    ret[index * ELEM_CNT + 1] = timezoneInfoList[index].startMinute;
                    ret[index * ELEM_CNT + 2] = timezoneInfoList[index].endHour;
                    ret[index * ELEM_CNT + 3] = timezoneInfoList[index].endMinute;
                }
            }
            return ret;
        }
        
        public void setItemData(int index, DbTimeSection timeSection)
        {
            if(index < 0 || index > TIME_ZONE_LIST_SIZE)
                return;
            
            timezoneInfoList[index] = timeSection;
            
            fireContentsChanged(this, index, index);
        }
        
        public int getSize() {
            return TIME_ZONE_LIST_SIZE;
        }

        public Object getElementAt(int index) {
            return timezoneInfoList[index];
        }

    }
    
    TimeZoneListModel listModel = new TimeZoneListModel();
    
    /** Creates new form AccessTzFrame */
    public AccessTzFrame() {
        initComponents();
    }
    
    /** This method is called from within the constructor to
     * initialize the form.
     * WARNING: Do NOT modify this code. The content of this method is
     * always regenerated by the Form Editor.
     */
    @SuppressWarnings("unchecked")
    // <editor-fold defaultstate="collapsed" desc="Generated Code">//GEN-BEGIN:initComponents
    private void initComponents() {

        lblMessage = new javax.swing.JLabel();
        jLabel1 = new javax.swing.JLabel();
        txtEnd = new javax.swing.JFormattedTextField();
        txtStart = new javax.swing.JFormattedTextField();
        jLabel3 = new javax.swing.JLabel();
        jScrollPane1 = new javax.swing.JScrollPane();
        lstTimezoneList = new javax.swing.JList();
        btnExit = new javax.swing.JButton();
        btnUpdate = new javax.swing.JButton();
        btnRead = new javax.swing.JButton();
        btnWrite = new javax.swing.JButton();

        setDefaultCloseOperation(javax.swing.WindowConstants.DISPOSE_ON_CLOSE);
        org.jdesktop.application.ResourceMap resourceMap = org.jdesktop.application.Application.getInstance(smack.comm.sample.MainFrame.class).getContext().getResourceMap(AccessTzFrame.class);
        setTitle(resourceMap.getString("Form.title")); // NOI18N
        setName("Form"); // NOI18N
        setResizable(false);
        addWindowListener(new java.awt.event.WindowAdapter() {
            public void windowClosed(java.awt.event.WindowEvent evt) {
                formWindowClosed(evt);
            }
        });
        getContentPane().setLayout(new org.netbeans.lib.awtextra.AbsoluteLayout());

        lblMessage.setFont(resourceMap.getFont("lblMessage.font")); // NOI18N
        lblMessage.setHorizontalAlignment(javax.swing.SwingConstants.CENTER);
        lblMessage.setText(resourceMap.getString("lblMessage.text")); // NOI18N
        lblMessage.setBorder(javax.swing.BorderFactory.createBevelBorder(javax.swing.border.BevelBorder.LOWERED));
        lblMessage.setName("lblMessage"); // NOI18N
        getContentPane().add(lblMessage, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 11, 553, 31));

        jLabel1.setText(resourceMap.getString("jLabel1.text")); // NOI18N
        jLabel1.setName("jLabel1"); // NOI18N
        getContentPane().add(jLabel1, new org.netbeans.lib.awtextra.AbsoluteConstraints(20, 90, -1, 20));

        txtEnd.setFormatterFactory(new javax.swing.text.DefaultFormatterFactory(new javax.swing.text.DateFormatter(new java.text.SimpleDateFormat("HH:mm"))));
        txtEnd.setText(resourceMap.getString("txtEnd.text")); // NOI18N
        txtEnd.setFont(resourceMap.getFont("txtStart.font")); // NOI18N
        txtEnd.setName("txtEnd"); // NOI18N
        getContentPane().add(txtEnd, new org.netbeans.lib.awtextra.AbsoluteConstraints(60, 90, 90, -1));

        txtStart.setFormatterFactory(new javax.swing.text.DefaultFormatterFactory(new javax.swing.text.DateFormatter(new java.text.SimpleDateFormat("HH:mm"))));
        txtStart.setFont(resourceMap.getFont("txtStart.font")); // NOI18N
        txtStart.setName("txtStart"); // NOI18N
        getContentPane().add(txtStart, new org.netbeans.lib.awtextra.AbsoluteConstraints(60, 60, 90, -1));

        jLabel3.setText(resourceMap.getString("jLabel3.text")); // NOI18N
        jLabel3.setName("jLabel3"); // NOI18N
        getContentPane().add(jLabel3, new org.netbeans.lib.awtextra.AbsoluteConstraints(20, 60, -1, 20));

        jScrollPane1.setName("jScrollPane1"); // NOI18N

        lstTimezoneList.setModel(listModel);
        lstTimezoneList.setSelectionMode(javax.swing.ListSelectionModel.SINGLE_SELECTION);
        lstTimezoneList.setName("lstTimezoneList"); // NOI18N
        lstTimezoneList.addMouseListener(new java.awt.event.MouseAdapter() {
            public void mouseClicked(java.awt.event.MouseEvent evt) {
                lstTimezoneListMouseClicked(evt);
            }
        });
        jScrollPane1.setViewportView(lstTimezoneList);

        getContentPane().add(jScrollPane1, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 120, 450, 350));

        btnExit.setFont(resourceMap.getFont("btnExit.font")); // NOI18N
        btnExit.setText(resourceMap.getString("btnExit.text")); // NOI18N
        btnExit.setName("btnExit"); // NOI18N
        btnExit.addMouseListener(new java.awt.event.MouseAdapter() {
            public void mouseClicked(java.awt.event.MouseEvent evt) {
                btnExitMouseClicked(evt);
            }
        });
        getContentPane().add(btnExit, new org.netbeans.lib.awtextra.AbsoluteConstraints(470, 430, 90, 40));

        btnUpdate.setFont(resourceMap.getFont("btnUpdate.font")); // NOI18N
        btnUpdate.setText(resourceMap.getString("btnUpdate.text")); // NOI18N
        btnUpdate.setName("btnUpdate"); // NOI18N
        btnUpdate.addMouseListener(new java.awt.event.MouseAdapter() {
            public void mouseClicked(java.awt.event.MouseEvent evt) {
                btnUpdateMouseClicked(evt);
            }
        });
        getContentPane().add(btnUpdate, new org.netbeans.lib.awtextra.AbsoluteConstraints(470, 120, 90, 40));

        btnRead.setFont(resourceMap.getFont("btnRead.font")); // NOI18N
        btnRead.setText(resourceMap.getString("btnRead.text")); // NOI18N
        btnRead.setName("btnRead"); // NOI18N
        btnRead.addMouseListener(new java.awt.event.MouseAdapter() {
            public void mouseClicked(java.awt.event.MouseEvent evt) {
                btnReadMouseClicked(evt);
            }
        });
        getContentPane().add(btnRead, new org.netbeans.lib.awtextra.AbsoluteConstraints(470, 330, 90, 40));

        btnWrite.setFont(resourceMap.getFont("btnWrite.font")); // NOI18N
        btnWrite.setText(resourceMap.getString("btnWrite.text")); // NOI18N
        btnWrite.setName("btnWrite"); // NOI18N
        btnWrite.addMouseListener(new java.awt.event.MouseAdapter() {
            public void mouseClicked(java.awt.event.MouseEvent evt) {
                btnWriteMouseClicked(evt);
            }
        });
        getContentPane().add(btnWrite, new org.netbeans.lib.awtextra.AbsoluteConstraints(470, 380, 90, 40));

        java.awt.Dimension screenSize = java.awt.Toolkit.getDefaultToolkit().getScreenSize();
        setBounds((screenSize.width-581)/2, (screenSize.height-521)/2, 581, 521);
    }// </editor-fold>//GEN-END:initComponents

    private void btnExitMouseClicked(java.awt.event.MouseEvent evt) {//GEN-FIRST:event_btnExitMouseClicked
        this.dispose();
    }//GEN-LAST:event_btnExitMouseClicked

    private void lstTimezoneListMouseClicked(java.awt.event.MouseEvent evt) {//GEN-FIRST:event_lstTimezoneListMouseClicked

        DbTimeSection timeSection = (DbTimeSection)lstTimezoneList.getSelectedValue();
        
        if(timeSection == null)
            return;
        
        Calendar calendar = Calendar.getInstance();
        calendar.set(Calendar.HOUR_OF_DAY, (int)timeSection.startHour);
        calendar.set(Calendar.MINUTE, (int)timeSection.startMinute);
        txtStart.setValue(calendar.getTime());
        
        calendar.set(Calendar.HOUR_OF_DAY, (int)timeSection.endHour);
        calendar.set(Calendar.MINUTE, (int)timeSection.endMinute);
        txtEnd.setValue(calendar.getTime());
    }//GEN-LAST:event_lstTimezoneListMouseClicked

    private void btnUpdateMouseClicked(java.awt.event.MouseEvent evt) {//GEN-FIRST:event_btnUpdateMouseClicked
        
        int index = lstTimezoneList.getSelectedIndex();
        if(index == -1)
            return;

        int timeZoneIndex = index / TimeZoneListModel.TIME_SECTION_COUNT;
        int timeSectionIndex  = index % TimeZoneListModel.TIME_SECTION_COUNT;
        DbTimeSection timeSection = new DbTimeSection(timeZoneIndex, timeSectionIndex);
        
        Calendar calendar = Calendar.getInstance();
        
        calendar.setTime((Date)txtStart.getValue());
        timeSection.startHour = calendar.get(Calendar.HOUR_OF_DAY);
        timeSection.startMinute = calendar.get(Calendar.MINUTE);
        
        calendar.setTime((Date)txtEnd.getValue());
        timeSection. endHour = calendar.get(Calendar.HOUR_OF_DAY);
        timeSection.endMinute = calendar.get(Calendar.MINUTE);
        
        listModel.setItemData(lstTimezoneList.getSelectedIndex(), timeSection);
    }//GEN-LAST:event_btnUpdateMouseClicked

    private void formWindowClosed(java.awt.event.WindowEvent evt) {//GEN-FIRST:event_formWindowClosed
       MainFrame.getApplication().getMainFrame().setVisible(true);
    }//GEN-LAST:event_formWindowClosed

    private void btnReadMouseClicked(java.awt.event.MouseEvent evt) {//GEN-FIRST:event_btnReadMouseClicked
        boolean ret;
        int errorCode;
        
        lblMessage.setText(SysUtil.WORKING);
        invalidate();
        
        ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, false);
        
        if(!ret)
        {
            lblMessage.setText(SysUtil.NO_DEVICE);
            return;
        }
        
        GetDeviceLongInfoOutput output;
        output = SBXPCProxy.GetDeviceLongInfo(SysUtil.MachineNumber, 3);
        
        if(output.isSuccess())
        {
            listModel.setAllData(output);
            errorCode = 0;
        }else
        {
            errorCode = (int)SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;
        }
        
        lblMessage.setText(SysUtil.ErrorPrint(errorCode));
        
        ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, true);
    }//GEN-LAST:event_btnReadMouseClicked

    private void btnWriteMouseClicked(java.awt.event.MouseEvent evt) {//GEN-FIRST:event_btnWriteMouseClicked
        boolean ret;
        int errorCode;
        
        lblMessage.setText(SysUtil.WORKING);
        invalidate();
        
        ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, false);
        
        if(!ret)
        {
            lblMessage.setText(SysUtil.NO_DEVICE);
            return;
        }
        
        ret = SBXPCProxy.SetDeviceLongInfo(SysUtil.MachineNumber, 3, listModel.getAllData());
        if(ret)
            errorCode = 0;
        else
            errorCode = (int)SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;
        
        lblMessage.setText(SysUtil.ErrorPrint(errorCode));
        
        ret = SBXPCProxy.EnableDevice(SysUtil.MachineNumber, true);
    }//GEN-LAST:event_btnWriteMouseClicked

    // Variables declaration - do not modify//GEN-BEGIN:variables
    private javax.swing.JButton btnExit;
    private javax.swing.JButton btnRead;
    private javax.swing.JButton btnUpdate;
    private javax.swing.JButton btnWrite;
    private javax.swing.JLabel jLabel1;
    private javax.swing.JLabel jLabel3;
    private javax.swing.JScrollPane jScrollPane1;
    private javax.swing.JLabel lblMessage;
    private javax.swing.JList lstTimezoneList;
    private javax.swing.JFormattedTextField txtEnd;
    private javax.swing.JFormattedTextField txtStart;
    // End of variables declaration//GEN-END:variables
}