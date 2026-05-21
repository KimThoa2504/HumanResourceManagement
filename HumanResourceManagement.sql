create database if not exists HumanResourceManagement;
use HumanResourceManagement;
-- bảng users
CREATE TABLE users (
    id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL,
    email VARCHAR(100) UNIQUE,
    phone VARCHAR(20),
    role ENUM('ADMIN', 'HR', 'MANAGER', 'VIEWER') NOT NULL,
    status ENUM('ACTIVE', 'INACTIVE', 'LOCKED') DEFAULT 'ACTIVE',
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);
-- bảng phòng ban
CREATE TABLE Departments (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) UNIQUE
);
ALTER TABLE Departments
ADD description VARCHAR(255);
-- bảng nhân viên
CREATE TABLE Employees (
    id INT AUTO_INCREMENT PRIMARY KEY,
    full_name VARCHAR(255) NOT NULL,
    dob DATE,
    gender ENUM('Male', 'Female', 'Other'),
    phone VARCHAR(20),
    address TEXT,
    department VARCHAR(100),
    position VARCHAR(100),
    hire_date DATE,
    salary DECIMAL(10,2),
    department_id INT,
    status ENUM('WORKING', 'RESIGNED', 'ONLEAVE') DEFAULT 'WORKING',
    FOREIGN KEY (department_id) REFERENCES Departments(id)
);
-- bảng chấm công
CREATE TABLE Attendance (
    id INT AUTO_INCREMENT PRIMARY KEY, -- ID chấm công
    employee_id INT NOT NULL, -- Nhân viên
    work_date DATE NOT NULL, -- Ngày làm việc
    check_in TIME, -- Giờ vào
    check_out TIME, -- Giờ ra
	UNIQUE (employee_id, work_date),
    FOREIGN KEY (employee_id) REFERENCES Employees(id)
);
-- bảng nghỉ phép
CREATE TABLE LeaveRequests (
    id INT AUTO_INCREMENT PRIMARY KEY, -- ID đơn nghỉ
    employee_id INT NOT NULL, -- Nhân viên
    start_date DATE NOT NULL, -- Ngày bắt đầu
    end_date DATE NOT NULL, -- Ngày kết thúc
    reason TEXT, -- Lý do nghỉ
    status ENUM('Pending', 'Approved', 'Rejected') DEFAULT 'Pending', -- Trạng thái
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP, -- Ngày tạo đơn
    FOREIGN KEY (employee_id) REFERENCES Employees(id)
);
-- bảng đánh giá
CREATE TABLE Performance (
    id INT AUTO_INCREMENT PRIMARY KEY, -- ID đánh giá
    employee_id INT NOT NULL, -- Nhân viên
    score INT CHECK (score BETWEEN 0 AND 100), -- Điểm đánh giá
    review TEXT, -- Nhận xét
    period VARCHAR(50), -- Ví dụ: '2026-Q1', '03-2026'
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP, -- Ngày đánh giá
    FOREIGN KEY (employee_id) REFERENCES Employees(id)
);
-- bảng tuyển dụng
CREATE TABLE Recruitment (
    id INT AUTO_INCREMENT PRIMARY KEY, -- ID tin tuyển dụng
    title VARCHAR(255), -- Tiêu đề
    description TEXT, -- Mô tả công việc
    department VARCHAR(100), -- Phòng ban tuyển
    status ENUM('OPEN', 'CLOSED') DEFAULT 'OPEN', -- Trạng thái
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP -- Ngày tạo
);
-- bảng ứng viên
CREATE TABLE Candidates (
    id INT AUTO_INCREMENT PRIMARY KEY, -- ID ứng viên
    full_name VARCHAR(255), -- Họ tên
    email VARCHAR(100),
    phone VARCHAR(20),
    recruitment_id INT, -- Thuộc tin tuyển dụng
    status ENUM('APPLIED', 'INTERVIEW', 'HIRED', 'REJECTED') DEFAULT 'APPLIED', -- Trạng thái
    FOREIGN KEY (recruitment_id) REFERENCES Recruitment(id)
);

