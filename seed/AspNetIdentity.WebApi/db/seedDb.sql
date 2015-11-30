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
	`SkypeHandle` VARCHAR(128) NULL COLLATE 'utf8_general_ci',
	`IndividualDescription` VARCHAR(2000) NULL COLLATE 'utf8_general_ci',
	`PhoneNumber` VARCHAR(50) NULL COLLATE 'utf8_general_ci',
	`Id` VARCHAR(128) NULL COLLATE 'latin1_swedish_ci',
	`Email` VARCHAR(256) NULL COLLATE 'latin1_swedish_ci',
	`UserName` VARCHAR(256) NULL COLLATE 'latin1_swedish_ci',
	`CategoryName` VARCHAR(50) NULL COLLATE 'utf8_general_ci'
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

-- Dumping data for table jv.aspnetuserroles: ~12 rows (approximately)
DELETE FROM `aspnetuserroles`;
/*!40000 ALTER TABLE `aspnetuserroles` DISABLE KEYS */;
INSERT INTO `aspnetuserroles` (`UserId`, `RoleId`) VALUES
	('1a5dccd4-9c41-4567-9801-c9b86d11de54', '1'),
	('619de077-a4ed-4421-84b2-e40bf9aa168b', '1'),
	('619de077-a4ed-4421-84b2-e40bf9aa168b', '2'),
	('686c419b-9b55-41cf-9b56-a52dece606bb', '1'),
	('686c419b-9b55-41cf-9b56-a52dece606bb', '2'),
	('8fa8d23f-7840-436f-9cd6-2eb888e62175', '1'),
	('8fa8d23f-7840-436f-9cd6-2eb888e62175', '2'),
	('93ba71dd-411c-4978-a2ff-4c2d659eb885', '2'),
	('b48bede0-8473-4f48-b72f-1380373ce280', '2'),
	('deada506-32ec-48de-98d5-195b36512171', '1'),
	('deada506-32ec-48de-98d5-195b36512171', '2'),
	('df21fceb-5f32-4d91-9796-d03b1bed061a', '2');
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

-- Dumping data for table jv.aspnetusers: ~8 rows (approximately)
DELETE FROM `aspnetusers`;
/*!40000 ALTER TABLE `aspnetusers` DISABLE KEYS */;
INSERT INTO `aspnetusers` (`Id`, `Email`, `EmailConfirmed`, `PasswordHash`, `SecurityStamp`, `PhoneNumber`, `PhoneNumberConfirmed`, `TwoFactorEnabled`, `LockoutEndDateUtc`, `LockoutEnabled`, `AccessFailedCount`, `UserName`) VALUES
	('1a5dccd4-9c41-4567-9801-c9b86d11de54', 'b@jv.com', 1, 'AIvLNYmHpkSBL0inS4rdWLT2xDjD7mQUDrxaBG+Sv8cRvdX7XA++NGvPCY60mzbmug==', '8ecc6d61-f65c-4ff9-a27d-c83cd18eb05c', '', 0, 0, NULL, 1, 0, 'billaff'),
	('619de077-a4ed-4421-84b2-e40bf9aa168b', 'jb4@jv.com', 1, 'APcAdy3SgRyWcLR4Oxno4e3yN9Tq2RgJIY6InQsxoYqzFnTuUnIFmAQvgevgfxvq4g==', '15a7ff01-faed-4614-955a-fced153e7f90', '', 0, 0, NULL, 1, 0, 'jbrady4'),
	('686c419b-9b55-41cf-9b56-a52dece606bb', 'jdersz@jv.com', 0, 'ANQ5sZeWYHk5lte/wCAnM2Qfi/gRU+TOftCVCfJZ9hCs4mVtld1gNED+CUwFpeaNGA==', '45be41b7-2b9b-4510-b74f-8641777b63d7', '', 0, 0, NULL, 1, 0, 'jdudersss'),
	('8fa8d23f-7840-436f-9cd6-2eb888e62175', 'd@csiportals.com', 1, 'APsRdYgHNVD8kPw9fg5TFY8hwGVxd4ZCHBuCrzJOrNHHyTFdwT5Xr1+oG0xjcnBbeQ==', '83a377c9-6688-46c9-bbcc-28912837d204', NULL, 0, 0, NULL, 1, 0, 'dawn'),
	('93ba71dd-411c-4978-a2ff-4c2d659eb885', 'm@jv.com', 1, 'AMQbVnJZ3XxihdwyvJqkoZ3SkcnSD5rP3p4ulrMcLDWuSKYSGIa9gXlKLEupe8ffyg==', '5056b8dc-d9ca-44eb-bea2-7441feb5ee1d', '', 0, 0, NULL, 1, 0, 'mollym'),
	('b48bede0-8473-4f48-b72f-1380373ce280', 'jders@jv.com', 0, 'AHvvzQPdT0f4flt8Xw7pjeWI8zUeiF7KD8SKwJPxZL+tMeOeG2VBTDor9sArwsHsBg==', 'd9fd2aca-f10a-45ad-bfe9-afd5d811692c', '', 0, 0, NULL, 1, 0, 'jduders'),
	('deada506-32ec-48de-98d5-195b36512171', 'bd@jv.com', 1, 'AGkBjjQKyHvxOtcDRej2hGDKyxoMtHQ6dgAkp/BonoHKdA78ovHjXnADuTAaKdNW7Q==', 'a0f08e2e-b6ad-44c4-afa5-c854d51f1e27', '', 0, 0, NULL, 1, 0, 'bdual'),
	('df21fceb-5f32-4d91-9796-d03b1bed061a', 'jd@jv.com', 0, 'AICTUqcAeDdVZLmVVs4aP+KWIwmsC4+fcrK9243TT/TA2ORDCadXBE5VYmDFnXOGuw==', 'd0148f01-7c04-4b8f-b2ae-1c68cd118444', '', 0, 0, NULL, 1, 0, 'jdude');
/*!40000 ALTER TABLE `aspnetusers` ENABLE KEYS */;


-- Dumping structure for table jv.categories
CREATE TABLE IF NOT EXISTS `categories` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8;

-- Dumping data for table jv.categories: ~13 rows (approximately)
DELETE FROM `categories`;
/*!40000 ALTER TABLE `categories` DISABLE KEYS */;
INSERT INTO `categories` (`Id`, `Name`) VALUES
	(1, 'Internet Marketing'),
	(2, 'Personal Development'),
	(3, 'Health and Wellness'),
	(4, 'Business Opportunity'),
	(5, 'Weight Loss'),
	(6, 'Dating and Relationships'),
	(7, 'Women\'s Health'),
	(8, 'Personal Finance'),
	(9, 'Investing and Trading'),
	(10, 'Technology'),
	(11, 'Beauty'),
	(12, 'Survival'),
	(13, 'Woodworking');
/*!40000 ALTER TABLE `categories` ENABLE KEYS */;


-- Dumping structure for view jv.marketerusers
-- Creating temporary table to overcome VIEW dependency errors
CREATE TABLE `marketerusers` (
	`id` VARCHAR(128) NOT NULL COLLATE 'latin1_swedish_ci'
) ENGINE=MyISAM;


-- Dumping structure for table jv.programs
CREATE TABLE IF NOT EXISTS `programs` (
  `Name` varchar(50) NOT NULL,
  `Category` int(11) NOT NULL DEFAULT '1',
  `Description` varchar(4000) NOT NULL,
  `Url` varchar(4000) NOT NULL,
  `CreatedDate` datetime NOT NULL,
  `CreatorId` varchar(128) NOT NULL,
  PRIMARY KEY (`Name`),
  KEY `FK_programs_category` (`Category`),
  CONSTRAINT `FK_programs_category` FOREIGN KEY (`category`) REFERENCES `categories` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Dumping data for table jv.programs: ~5 rows (approximately)
DELETE FROM `programs`;
/*!40000 ALTER TABLE `programs` DISABLE KEYS */;
INSERT INTO `programs` (`Name`, `Category`, `Description`, `Url`, `CreatedDate`, `CreatorId`) VALUES
	('Live It Up!', 1, 'Every person has a dream - I support that - and I dream with them - in solidarity - every day - like there\'s no tomorrow.', 'http://cnn.com', '2015-11-12 14:59:27', 'a3499e97-37b3-4153-97f6-01d8d7e5caf6'),
	('Maid in Heaven', 1, 'If you want to strive to get to move up the ladder out of manual labor, sell for me', 'http://mollymaid.com', '2015-11-15 22:03:59', '93ba71dd-411c-4978-a2ff-4c2d659eb885'),
	('Soccer for the masses', 1, 'Briningin soccer to underserved neighborhoods charging .50 a game', 'http://cnn.com', '2015-11-16 06:25:43', 'deada506-32ec-48de-98d5-195b36512171'),
	('The Biggie', 1, 'This is a good opportunity for a work from home professional with lots of initiative', 'http://www.cnn.com', '2015-11-15 16:22:01', '619de077-a4ed-4421-84b2-e40bf9aa168b'),
	('The Biggiez', 1, ';lkj;lkj ;lkj ;lkj;lkj ;lkj;lkj ;lkj;l ;lkj;lkj ;lkj;l j;lkj;l kj;lk j', 'http://www.cnn.com', '2015-11-19 21:06:59', '686c419b-9b55-41cf-9b56-a52dece606bb');
/*!40000 ALTER TABLE `programs` ENABLE KEYS */;


-- Dumping structure for table jv.userextensions
CREATE TABLE IF NOT EXISTS `userextensions` (
  `UserID` varchar(128) NOT NULL,
  `SkypeHandle` varchar(128) NOT NULL,
  `IndividualDescription` varchar(2000) NOT NULL,
  `FirstName` varchar(50) DEFAULT NULL,
  `LastName` varchar(50) DEFAULT NULL,
  `PhoneNumber` varchar(50) DEFAULT NULL,
  `Credits` int(10) unsigned NOT NULL,
  `Category` int(11) DEFAULT '1',
  UNIQUE KEY `UserID` (`UserID`),
  KEY `FK_userextensions_category` (`Category`),
  CONSTRAINT `FK_userextensions_category` FOREIGN KEY (`Category`) REFERENCES `categories` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Dumping data for table jv.userextensions: ~8 rows (approximately)
DELETE FROM `userextensions`;
/*!40000 ALTER TABLE `userextensions` DISABLE KEYS */;
INSERT INTO `userextensions` (`UserID`, `SkypeHandle`, `IndividualDescription`, `FirstName`, `LastName`, `PhoneNumber`, `Credits`, `Category`) VALUES
	('1a5dccd4-9c41-4567-9801-c9b86d11de54', 'billaff', 'just a regular guy looking for a regular paycheck I\'m pretty middle of the road', 'Bill', 'Affiliate', '9197405777', 4, 2),
	('619de077-a4ed-4421-84b2-e40bf9aa168b', 'jfbchill', 'Just the best ol guy you could find in the scrap yard - nothing too simple here.', 'John', 'Brady', '919-222-1313', 1, 6),
	('686c419b-9b55-41cf-9b56-a52dece606bb', 'duderoo', 'l;j;lkj ;lkj;lk ;lkj ;lkj ;lkj ;lkj ;ljkj ;lkj ;lkj ;lkj ;lkj ;lk', 'john', 'dude', '9197405777', 0, 7),
	('8fa8d23f-7840-436f-9cd6-2eb888e62175', 'dconskype', 'A dynamo sales person, also partner in a three person web marketing digigal agency', 'Dawn', 'Cassara', '812-312-1234', 0, 12),
	('93ba71dd-411c-4978-a2ff-4c2d659eb885', 'mollym', 'molly maid ex team lead likes to work days for extra cash outside the cleaning industry', 'Molly', 'Marketer', '9197405777', 0, 9),
	('b48bede0-8473-4f48-b72f-1380373ce280', 'duderoo', 'l;j;lkj ;lkj;lk ;lkj ;lkj ;lkj ;lkj ;ljkj ;lkj ;lkj ;lkj ;lkj ;lk', 'john', 'dude', '9197405777', 0, 8),
	('deada506-32ec-48de-98d5-195b36512171', 'atleast6', 'Bob Dual is a great salesman father, husband and soccer player and so many much mores to the alls in the communities around him', 'Bob', 'Dual', '9197405777', 0, 12),
	('df21fceb-5f32-4d91-9796-d03b1bed061a', 'duderoo', 'l;j;lkj ;lkj;lk ;lkj ;lkj ;lkj ;lkj ;ljkj ;lkj ;lkj ;lkj ;lkj ;lk', 'john', 'dude', '9197405777', 0, 5);
/*!40000 ALTER TABLE `userextensions` ENABLE KEYS */;


-- Dumping structure for table jv.userprogramunlocks
CREATE TABLE IF NOT EXISTS `userprogramunlocks` (
  `PayingUser` varchar(128) NOT NULL,
  `ProgramName` varchar(50) NOT NULL,
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8;

-- Dumping data for table jv.userprogramunlocks: ~5 rows (approximately)
DELETE FROM `userprogramunlocks`;
/*!40000 ALTER TABLE `userprogramunlocks` DISABLE KEYS */;
INSERT INTO `userprogramunlocks` (`PayingUser`, `ProgramName`, `Id`) VALUES
	('619de077-a4ed-4421-84b2-e40bf9aa168b', 'Maid in Heaven', 1),
	('deada506-32ec-48de-98d5-195b36512171', 'Maid in Heaven', 7);
/*!40000 ALTER TABLE `userprogramunlocks` ENABLE KEYS */;


-- Dumping structure for table jv.userratings
CREATE TABLE IF NOT EXISTS `userratings` (
  `rater` varchar(128) DEFAULT NULL,
  `rated` varchar(128) DEFAULT NULL,
  `rating` int(11) DEFAULT NULL,
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8;

-- Dumping data for table jv.userratings: ~10 rows (approximately)
DELETE FROM `userratings`;
/*!40000 ALTER TABLE `userratings` DISABLE KEYS */;
INSERT INTO `userratings` (`rater`, `rated`, `rating`, `id`) VALUES
	('619de077-a4ed-4421-84b2-e40bf9aa168b', 'deada506-32ec-48de-98d5-195b36512171', 4, 1),
	('619de077-a4ed-4421-84b2-e40bf9aa168b', '686c419b-9b55-41cf-9b56-a52dece606bb', 1, 2),
	('619de077-a4ed-4421-84b2-e40bf9aa168b', '8fa8d23f-7840-436f-9cd6-2eb888e62175', 5, 3),
	('619de077-a4ed-4421-84b2-e40bf9aa168b', '1a5dccd4-9c41-4567-9801-c9b86d11de54', 0, 4),
	('619de077-a4ed-4421-84b2-e40bf9aa168b', '619de077-a4ed-4421-84b2-e40bf9aa168b', 4, 5),
	('93ba71dd-411c-4978-a2ff-4c2d659eb885', '619de077-a4ed-4421-84b2-e40bf9aa168b', 1, 6),
	('93ba71dd-411c-4978-a2ff-4c2d659eb885', '8fa8d23f-7840-436f-9cd6-2eb888e62175', 3, 7),
	('93ba71dd-411c-4978-a2ff-4c2d659eb885', 'deada506-32ec-48de-98d5-195b36512171', 1, 8),
	('93ba71dd-411c-4978-a2ff-4c2d659eb885', '1a5dccd4-9c41-4567-9801-c9b86d11de54', 3, 9),
	('93ba71dd-411c-4978-a2ff-4c2d659eb885', '686c419b-9b55-41cf-9b56-a52dece606bb', 5, 10);
/*!40000 ALTER TABLE `userratings` ENABLE KEYS */;


-- Dumping structure for table jv.useruserunlocks
CREATE TABLE IF NOT EXISTS `useruserunlocks` (
  `PayingUser` varchar(128) NOT NULL,
  `RevealedUser` varchar(128) NOT NULL,
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=46 DEFAULT CHARSET=utf8;

-- Dumping data for table jv.useruserunlocks: ~9 rows (approximately)
DELETE FROM `useruserunlocks`;
/*!40000 ALTER TABLE `useruserunlocks` DISABLE KEYS */;
INSERT INTO `useruserunlocks` (`PayingUser`, `RevealedUser`, `Id`) VALUES
	('619de077-a4ed-4421-84b2-e40bf9aa168b', '1a5dccd4-9c41-4567-9801-c9b86d11de54', 4),
	('619de077-a4ed-4421-84b2-e40bf9aa168b', '619de077-a4ed-4421-84b2-e40bf9aa168b', 5),
	('619de077-a4ed-4421-84b2-e40bf9aa168b', '686c419b-9b55-41cf-9b56-a52dece606bb', 6),
	('619de077-a4ed-4421-84b2-e40bf9aa168b', '8fa8d23f-7840-436f-9cd6-2eb888e62175', 7),
	('619de077-a4ed-4421-84b2-e40bf9aa168b', 'deada506-32ec-48de-98d5-195b36512171', 8),
	('93ba71dd-411c-4978-a2ff-4c2d659eb885', '1a5dccd4-9c41-4567-9801-c9b86d11de54', 39),
	('93ba71dd-411c-4978-a2ff-4c2d659eb885', '619de077-a4ed-4421-84b2-e40bf9aa168b', 40),
	('93ba71dd-411c-4978-a2ff-4c2d659eb885', '686c419b-9b55-41cf-9b56-a52dece606bb', 41),
	('93ba71dd-411c-4978-a2ff-4c2d659eb885', '8fa8d23f-7840-436f-9cd6-2eb888e62175', 42),
	('deada506-32ec-48de-98d5-195b36512171', '1a5dccd4-9c41-4567-9801-c9b86d11de54', 45);
/*!40000 ALTER TABLE `useruserunlocks` ENABLE KEYS */;


-- Dumping structure for view jv.affiliateusers
-- Removing temporary table and create final VIEW structure
DROP TABLE IF EXISTS `affiliateusers`;
CREATE ALGORITHM=UNDEFINED DEFINER=`dev`@`127.0.0.1` VIEW `affiliateusers` AS SELECT ux.FirstName, ux.LastName, ux.SkypeHandle, ux.IndividualDescription, ux.PhoneNumber, u.Id, u.Email, u.UserName, c.name as CategoryName 
 from aspnetroles r inner join aspnetuserroles ur on ur.RoleId = r.Id 
inner join aspnetusers u on u.Id = ur.UserId inner join userextensions ux on ux.UserID = u.Id right join categories c on c.Id = ux.Category where r.Id = 1 ;


-- Dumping structure for view jv.marketerusers
-- Removing temporary table and create final VIEW structure
DROP TABLE IF EXISTS `marketerusers`;
CREATE ALGORITHM=UNDEFINED DEFINER=`dev`@`127.0.0.1` VIEW `marketerusers` AS SELECT u.id from aspnetroles r inner join aspnetuserroles ur on ur.RoleId = r.Id
inner join aspnetusers u on u.Id = ur.UserId where r.Id = 2 ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
