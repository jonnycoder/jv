-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               5.5.38-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win64
-- HeidiSQL Version:             8.3.0.4694
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping database structure for jv
CREATE DATABASE IF NOT EXISTS `jv` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `jv`;


-- Dumping structure for view jv.affiliateusers
-- Creating temporary table to overcome VIEW dependency errors
CREATE TABLE `affiliateusers` (
	`FirstName` VARCHAR(50) NULL COLLATE 'utf8_general_ci',
	`LastName` VARCHAR(50) NULL COLLATE 'utf8_general_ci',
	`SkypeHandle` VARCHAR(128) NOT NULL COLLATE 'utf8_general_ci',
	`IndividualDescription` VARCHAR(2000) NOT NULL COLLATE 'utf8_general_ci',
	`PhoneNumber` VARCHAR(50) NULL COLLATE 'utf8_general_ci',
	`Id` VARCHAR(128) NOT NULL COLLATE 'latin1_swedish_ci',
	`Email` VARCHAR(256) NULL COLLATE 'latin1_swedish_ci',
	`UserName` VARCHAR(256) NOT NULL COLLATE 'latin1_swedish_ci'
) ENGINE=MyISAM;


-- Dumping structure for table jv.aspnetroles
CREATE TABLE IF NOT EXISTS `aspnetroles` (
  `Id` varchar(128) NOT NULL,
  `Name` varchar(256) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table jv.aspnetroles: ~3 rows (approximately)
DELETE FROM `aspnetroles`;
/*!40000 ALTER TABLE `aspnetroles` DISABLE KEYS */;
INSERT INTO `aspnetroles` (`Id`, `Name`) VALUES
	('0', 'Admin'),
	('1', 'Affiliate'),
	('2', 'Vendor');
/*!40000 ALTER TABLE `aspnetroles` ENABLE KEYS */;


-- Dumping structure for table jv.aspnetuserclaims
CREATE TABLE IF NOT EXISTS `aspnetuserclaims` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` varchar(128) NOT NULL,
  `ClaimType` longtext,
  `ClaimValue` longtext,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id` (`Id`),
  KEY `UserId` (`UserId`),
  CONSTRAINT `ApplicationUser_Claims` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table jv.aspnetuserclaims: ~0 rows (approximately)
DELETE FROM `aspnetuserclaims`;
/*!40000 ALTER TABLE `aspnetuserclaims` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetuserclaims` ENABLE KEYS */;


-- Dumping structure for table jv.aspnetuserlogins
CREATE TABLE IF NOT EXISTS `aspnetuserlogins` (
  `LoginProvider` varchar(128) NOT NULL,
  `ProviderKey` varchar(128) NOT NULL,
  `UserId` varchar(128) NOT NULL,
  PRIMARY KEY (`LoginProvider`,`ProviderKey`,`UserId`),
  KEY `ApplicationUser_Logins` (`UserId`),
  CONSTRAINT `ApplicationUser_Logins` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table jv.aspnetuserlogins: ~0 rows (approximately)
DELETE FROM `aspnetuserlogins`;
/*!40000 ALTER TABLE `aspnetuserlogins` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetuserlogins` ENABLE KEYS */;


-- Dumping structure for table jv.aspnetuserroles
CREATE TABLE IF NOT EXISTS `aspnetuserroles` (
  `UserId` varchar(128) NOT NULL,
  `RoleId` varchar(128) NOT NULL,
  PRIMARY KEY (`UserId`,`RoleId`),
  KEY `IdentityRole_Users` (`RoleId`),
  CONSTRAINT `ApplicationUser_Roles` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT `IdentityRole_Users` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table jv.aspnetuserroles: ~2 rows (approximately)
DELETE FROM `aspnetuserroles`;
/*!40000 ALTER TABLE `aspnetuserroles` DISABLE KEYS */;
INSERT INTO `aspnetuserroles` (`UserId`, `RoleId`) VALUES
	('1a5dccd4-9c41-4567-9801-c9b86d11de54', '1'),
	('619de077-a4ed-4421-84b2-e40bf9aa168b', '1'),
	('619de077-a4ed-4421-84b2-e40bf9aa168b', '2'),
	('8fa8d23f-7840-436f-9cd6-2eb888e62175', '1'),
	('8fa8d23f-7840-436f-9cd6-2eb888e62175', '2'),
	('93ba71dd-411c-4978-a2ff-4c2d659eb885', '2');
/*!40000 ALTER TABLE `aspnetuserroles` ENABLE KEYS */;


-- Dumping structure for table jv.aspnetusers
CREATE TABLE IF NOT EXISTS `aspnetusers` (
  `Id` varchar(128) CHARACTER SET latin1 NOT NULL,
  `Email` varchar(256) CHARACTER SET latin1 DEFAULT NULL,
  `EmailConfirmed` tinyint(1) NOT NULL,
  `PasswordHash` longtext CHARACTER SET latin1,
  `SecurityStamp` longtext CHARACTER SET latin1,
  `PhoneNumber` longtext CHARACTER SET latin1,
  `PhoneNumberConfirmed` tinyint(1) NOT NULL,
  `TwoFactorEnabled` tinyint(1) NOT NULL,
  `LockoutEndDateUtc` datetime DEFAULT NULL,
  `LockoutEnabled` tinyint(1) NOT NULL,
  `AccessFailedCount` int(11) NOT NULL,
  `UserName` varchar(256) CHARACTER SET latin1 NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Dumping data for table jv.aspnetusers: ~23 rows (approximately)
DELETE FROM `aspnetusers`;
/*!40000 ALTER TABLE `aspnetusers` DISABLE KEYS */;
INSERT INTO `aspnetusers` (`Id`, `Email`, `EmailConfirmed`, `PasswordHash`, `SecurityStamp`, `PhoneNumber`, `PhoneNumberConfirmed`, `TwoFactorEnabled`, `LockoutEndDateUtc`, `LockoutEnabled`, `AccessFailedCount`, `UserName`) VALUES
	('1a5dccd4-9c41-4567-9801-c9b86d11de54', 'b@jv.com', 1, 'AIvLNYmHpkSBL0inS4rdWLT2xDjD7mQUDrxaBG+Sv8cRvdX7XA++NGvPCY60mzbmug==', '8ecc6d61-f65c-4ff9-a27d-c83cd18eb05c', '', 0, 0, NULL, 1, 0, 'billaff'),
	('619de077-a4ed-4421-84b2-e40bf9aa168b', 'jb4@jv.com', 1, 'APcAdy3SgRyWcLR4Oxno4e3yN9Tq2RgJIY6InQsxoYqzFnTuUnIFmAQvgevgfxvq4g==', '15a7ff01-faed-4614-955a-fced153e7f90', '', 0, 0, NULL, 1, 0, 'jbrady4'),
	('8fa8d23f-7840-436f-9cd6-2eb888e62175', 'd@csiportals.com', 1, 'APsRdYgHNVD8kPw9fg5TFY8hwGVxd4ZCHBuCrzJOrNHHyTFdwT5Xr1+oG0xjcnBbeQ==', '83a377c9-6688-46c9-bbcc-28912837d204', NULL, 0, 0, NULL, 1, 0, 'dawn'),
	('93ba71dd-411c-4978-a2ff-4c2d659eb885', 'm@jv.com', 1, 'AMQbVnJZ3XxihdwyvJqkoZ3SkcnSD5rP3p4ulrMcLDWuSKYSGIa9gXlKLEupe8ffyg==', '5056b8dc-d9ca-44eb-bea2-7441feb5ee1d', '', 0, 0, NULL, 1, 0, 'mollym');
/*!40000 ALTER TABLE `aspnetusers` ENABLE KEYS */;


-- Dumping structure for view jv.marketerusers
-- Creating temporary table to overcome VIEW dependency errors
CREATE TABLE `marketerusers` (
	`id` VARCHAR(128) NOT NULL COLLATE 'latin1_swedish_ci'
) ENGINE=MyISAM;


-- Dumping structure for table jv.programs
CREATE TABLE IF NOT EXISTS `programs` (
  `Name` varchar(50) NOT NULL,
  `Description` varchar(4000) NOT NULL,
  `Url` varchar(4000) NOT NULL,
  `CreatedDate` datetime NOT NULL,
  `CreatorId` varchar(128) NOT NULL,
  PRIMARY KEY (`Name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Dumping data for table jv.programs: ~1 rows (approximately)
DELETE FROM `programs`;
/*!40000 ALTER TABLE `programs` DISABLE KEYS */;
INSERT INTO `programs` (`Name`, `Description`, `Url`, `CreatedDate`, `CreatorId`) VALUES
	('Live It Up!', 'Every person has a dream - I support that - and I dream with them - in solidarity - every day - like there\'s no tomorrow.', 'http://cnn.com', '2015-11-12 14:59:27', 'a3499e97-37b3-4153-97f6-01d8d7e5caf6'),
	('Maid in Heaven', 'If you want to strive to get to move up the ladder out of manual labor, sell for me', 'http://mollymaid.com', '2015-11-15 22:03:59', '93ba71dd-411c-4978-a2ff-4c2d659eb885'),
	('The Biggie', 'This is a good opportunity for a work from home professional with lots of initiative', 'http://www.cnn.com', '2015-11-15 16:22:01', '619de077-a4ed-4421-84b2-e40bf9aa168b');
/*!40000 ALTER TABLE `programs` ENABLE KEYS */;


-- Dumping structure for table jv.userextensions
CREATE TABLE IF NOT EXISTS `userextensions` (
  `UserID` varchar(128) NOT NULL,
  `SkypeHandle` varchar(128) NOT NULL,
  `IndividualDescription` varchar(2000) NOT NULL,
  `FirstName` varchar(50) DEFAULT NULL,
  `LastName` varchar(50) DEFAULT NULL,
  `PhoneNumber` varchar(50) DEFAULT NULL,
  UNIQUE KEY `UserID` (`UserID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Dumping data for table jv.userextensions: ~8 rows (approximately)
DELETE FROM `userextensions`;
/*!40000 ALTER TABLE `userextensions` DISABLE KEYS */;
INSERT INTO `userextensions` (`UserID`, `SkypeHandle`, `IndividualDescription`, `FirstName`, `LastName`, `PhoneNumber`) VALUES
	('1a5dccd4-9c41-4567-9801-c9b86d11de54', 'billaff', 'just a regular guy looking for a regular paycheck I\'m pretty middle of the road', 'Bill', 'Affiliate', '9197405777'),
	('619de077-a4ed-4421-84b2-e40bf9aa168b', 'jfbchill', 'Just the best ol guy you could find in the scrap yard - nothing too simple here.', 'John', 'Brady', '919-222-1313'),
	('8fa8d23f-7840-436f-9cd6-2eb888e62175', 'dc', 'A dynamo sales person, also partner in a three person web marketing digigal agency', 'Dawn', 'Cassara', '812-312-1234'),
	('93ba71dd-411c-4978-a2ff-4c2d659eb885', 'mollym', 'molly maid ex team lead likes to work days for extra cash outside the cleaning industry', 'Molly', 'Marketer', '9197405777');
/*!40000 ALTER TABLE `userextensions` ENABLE KEYS */;


-- Dumping structure for view jv.affiliateusers
-- Removing temporary table and create final VIEW structure
DROP TABLE IF EXISTS `affiliateusers`;
CREATE ALGORITHM=UNDEFINED DEFINER=`dev`@`127.0.0.1` VIEW `affiliateusers` AS SELECT ux.FirstName, ux.LastName, ux.SkypeHandle, ux.IndividualDescription, ux.PhoneNumber, u.Id, u.Email, u.UserName  from aspnetroles r inner join aspnetuserroles ur on ur.RoleId = r.Id 
inner join aspnetusers u on u.Id = ur.UserId inner join userextensions ux on ux.UserID = u.Id where r.Id = 1 ;


-- Dumping structure for view jv.marketerusers
-- Removing temporary table and create final VIEW structure
DROP TABLE IF EXISTS `marketerusers`;
CREATE ALGORITHM=UNDEFINED DEFINER=`dev`@`127.0.0.1` VIEW `marketerusers` AS SELECT u.id from aspnetroles r inner join aspnetuserroles ur on ur.RoleId = r.Id
inner join aspnetusers u on u.Id = ur.UserId where r.Id = 2 ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
