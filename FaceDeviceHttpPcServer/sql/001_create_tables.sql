-- FaceDeviceHttpPcServer MySQL Schema
-- Version: 1.0
-- Charset: utf8mb4

CREATE DATABASE IF NOT EXISTS facedevice
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_unicode_ci;

USE facedevice;

-- ---------------------------------------------------------------------------
-- 1. people (사용자)
-- ---------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS people (
    user_id           VARCHAR(64)  NOT NULL PRIMARY KEY,
    code              VARCHAR(64)  NULL,
    name              VARCHAR(100) NULL,
    job               VARCHAR(100) NULL,
    department        VARCHAR(100) NULL,
    identity_card     VARCHAR(50)  NULL,
    attachment        VARCHAR(255) NULL,
    photo             LONGTEXT     NULL COMMENT 'Base64 or file path',
    photo_md5         VARCHAR(64)  NULL,
    photo_len         INT          NOT NULL DEFAULT 0,
    password          VARCHAR(100) NULL,
    card_num          VARCHAR(50)  NOT NULL DEFAULT '0',
    qr_code           VARCHAR(255) NULL,
    access_type       INT          NOT NULL DEFAULT 0,
    expiration_date   INT UNSIGNED NOT NULL DEFAULT 0,
    open_times        INT          NOT NULL DEFAULT 65535,
    keep_open         INT          NOT NULL DEFAULT 0,
    timegroup         INT          NOT NULL DEFAULT 1,
    holidays          VARCHAR(255) NULL,
    elevators         VARCHAR(255) NULL,
    face_feature      LONGTEXT     NULL,
    face_feature_md5  VARCHAR(64)  NULL,
    fingerprints_json JSON         NULL COMMENT 'List of FingerprintItem',
    palmveins_json    JSON         NULL COMMENT 'List of PalmveinItem',
    created_at        DATETIME(3)  NOT NULL DEFAULT CURRENT_TIMESTAMP(3),
    updated_at        DATETIME(3)  NOT NULL DEFAULT CURRENT_TIMESTAMP(3) ON UPDATE CURRENT_TIMESTAMP(3),
    INDEX idx_people_name (name),
    INDEX idx_people_department (department),
    INDEX idx_people_card (card_num)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ---------------------------------------------------------------------------
-- 2. devices (장비)
-- ---------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS devices (
    sn                            VARCHAR(64)  NOT NULL PRIMARY KEY,
    ip_address                    VARCHAR(45)  NULL,
    http_port                     INT          NOT NULL DEFAULT 80,
    device_name                   VARCHAR(100) NULL,
    tag_name                      VARCHAR(100) NULL,
    model                         VARCHAR(50)  NULL,
    firmware_version              VARCHAR(50)  NULL,
    unit_no                       INT          NOT NULL DEFAULT 0,
    connected_at                  DATETIME(3)  NULL,
    last_keepalive_at             DATETIME(3)  NULL,
    last_work_setting_upload_at   DATETIME(3)  NULL,
    last_keepalive_json           JSON         NULL COMMENT 'Last KeepaliveRequest',
    last_uploaded_work_setting    JSON         NULL,
    desired_work_setting          JSON         NULL,
    pending_sync_parameter        TINYINT(1)   NOT NULL DEFAULT 0,
    pending_upload_work_parameter TINYINT(1)   NOT NULL DEFAULT 0,
    pending_add_people_count      INT          NOT NULL DEFAULT 0,
    pending_remote_json           JSON         NULL COMMENT 'PendingRemoteCommand',
    created_at                    DATETIME(3)  NOT NULL DEFAULT CURRENT_TIMESTAMP(3),
    updated_at                    DATETIME(3)  NOT NULL DEFAULT CURRENT_TIMESTAMP(3) ON UPDATE CURRENT_TIMESTAMP(3),
    INDEX idx_devices_ip (ip_address),
    INDEX idx_devices_last_keepalive (last_keepalive_at)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ---------------------------------------------------------------------------
-- 3. device_people (장비별 배포/스테이징 사용자)
-- ---------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS device_people (
    device_sn    VARCHAR(64) NOT NULL,
    user_id      VARCHAR(64) NOT NULL,
    downloaded   TINYINT(1)  NOT NULL DEFAULT 0 COMMENT '1 = already sent to device',
    staged       TINYINT(1)  NOT NULL DEFAULT 0 COMMENT '1 = staged for this device only',
    owned        TINYINT(1)  NOT NULL DEFAULT 0 COMMENT '1 = currently registered on device',
    created_at   DATETIME(3) NOT NULL DEFAULT CURRENT_TIMESTAMP(3),
    updated_at   DATETIME(3) NOT NULL DEFAULT CURRENT_TIMESTAMP(3) ON UPDATE CURRENT_TIMESTAMP(3),
    PRIMARY KEY (device_sn, user_id),
    CONSTRAINT fk_dp_device FOREIGN KEY (device_sn) REFERENCES devices(sn) ON DELETE CASCADE,
    CONSTRAINT fk_dp_people FOREIGN KEY (user_id) REFERENCES people(user_id) ON DELETE CASCADE,
    INDEX idx_dp_user (user_id),
    INDEX idx_dp_staged (device_sn, staged),
    INDEX idx_dp_downloaded (device_sn, downloaded)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ---------------------------------------------------------------------------
-- 4. pending_deletes (삭제 대기열)
-- ---------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS pending_deletes (
    device_sn    VARCHAR(64) NOT NULL,
    user_id      VARCHAR(64) NOT NULL,
    created_at   DATETIME(3) NOT NULL DEFAULT CURRENT_TIMESTAMP(3),
    PRIMARY KEY (device_sn, user_id),
    CONSTRAINT fk_pd_device FOREIGN KEY (device_sn) REFERENCES devices(sn) ON DELETE CASCADE,
    INDEX idx_pd_device (device_sn)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ---------------------------------------------------------------------------
-- 5. deleted_user_ids (전역 삭제 이력 - 선택)
-- ---------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS deleted_user_ids (
    user_id      VARCHAR(64) NOT NULL PRIMARY KEY,
    deleted_at   DATETIME(3) NOT NULL DEFAULT CURRENT_TIMESTAMP(3)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ---------------------------------------------------------------------------
-- 6. identify_records (출입/인증 기록)
-- ---------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS identify_records (
    id              BIGINT       NOT NULL AUTO_INCREMENT PRIMARY KEY,
    device_sn       VARCHAR(64)  NOT NULL,
    record_id       VARCHAR(64)  NULL,
    user_id         VARCHAR(64)  NULL,
    user_name       VARCHAR(100) NULL,
    record_type     INT          NULL,
    record_time     DATETIME(3)  NULL,
    temperature     VARCHAR(20)  NULL,
    photo_path      VARCHAR(500) NULL,
    raw_json        JSON         NULL,
    received_at     DATETIME(3)  NOT NULL DEFAULT CURRENT_TIMESTAMP(3),
    INDEX idx_ir_device_time (device_sn, record_time),
    INDEX idx_ir_user_time (user_id, record_time),
    INDEX idx_ir_received (received_at),
    CONSTRAINT fk_ir_device FOREIGN KEY (device_sn) REFERENCES devices(sn) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
