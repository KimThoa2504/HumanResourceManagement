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
    id INT AUTO_INCREMENT PRIMARY KEY, 
    employee_id INT NOT NULL,
    work_date DATE NOT NULL, 
    check_in TIME, 
    check_out TIME, 
	UNIQUE (employee_id, work_date),
    FOREIGN KEY (employee_id) REFERENCES Employees(id)
);
-- bảng nghỉ phép
CREATE TABLE LeaveRequests (
    id INT AUTO_INCREMENT PRIMARY KEY, 
    employee_id INT NOT NULL, 
    start_date DATE NOT NULL, 
    end_date DATE NOT NULL, 
    reason TEXT, 
    status ENUM('Pending', 'Approved', 'Rejected') DEFAULT 'Pending', 
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP, 
    FOREIGN KEY (employee_id) REFERENCES Employees(id)
);
ALTER TABLE LeaveRequests
ADD COLUMN attachment_path VARCHAR(255),
ADD COLUMN attachment_name VARCHAR(255);
    
-- bảng đánh giá
CREATE TABLE Performance (
    id INT AUTO_INCREMENT PRIMARY KEY, 
    employee_id INT NOT NULL, 
    score INT CHECK (score BETWEEN 0 AND 100), 
    review TEXT, 
    period VARCHAR(50), 
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP, 
    FOREIGN KEY (employee_id) REFERENCES Employees(id)
);
-- bảng tuyển dụng
CREATE TABLE Recruitment (
    id INT AUTO_INCREMENT PRIMARY KEY, 
    title VARCHAR(255), 
    description TEXT, 
    department VARCHAR(100), 
    status ENUM('OPEN', 'CLOSED') DEFAULT 'OPEN', 
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP 
);
-- bảng ứng viên
CREATE TABLE Candidates (
    id INT AUTO_INCREMENT PRIMARY KEY, 
    full_name VARCHAR(255), 
    email VARCHAR(100),
    phone VARCHAR(20),
    recruitment_id INT, 
    status ENUM('APPLIED', 'INTERVIEW', 'HIRED', 'REJECTED') DEFAULT 'APPLIED', 
    FOREIGN KEY (recruitment_id) REFERENCES Recruitment(id)
);

