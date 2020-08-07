-- phpMyAdmin SQL Dump
-- version 4.8.2
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jun 14, 2020 at 04:18 PM
-- Server version: 10.1.34-MariaDB
-- PHP Version: 7.2.7

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `fingerprint`
--

-- --------------------------------------------------------

--
-- Table structure for table `fingerprint`
--

CREATE TABLE `fingerprint` (
  `IDFINGERPRINT` int(11) NOT NULL,
  `IDJADWAL` int(11) DEFAULT NULL,
  `IDIDENTITTAS` int(11) DEFAULT NULL,
  `NOMORFINGERPRINT` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `hystory`
--

CREATE TABLE `hystory` (
  `IDHYSTORY` int(11) NOT NULL,
  `INFORMASIHYSTORY` varchar(100) DEFAULT NULL,
  `TANGGALHYSTORY` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `identitas`
--

CREATE TABLE `identitas` (
  `IDIDENTITTAS` int(11) NOT NULL,
  `IDRULE` int(11) DEFAULT NULL,
  `NAMAIDENTITAS` varchar(100) DEFAULT NULL,
  `NOMORIDENTITAS` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `jadwal`
--

CREATE TABLE `jadwal` (
  `IDJADWAL` int(11) NOT NULL,
  `JAMJADWALMASUK` time DEFAULT NULL,
  `JAMJADWALKELUAR` time DEFAULT NULL,
  `HARIJADWAL` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `jadwalkuliah`
--

CREATE TABLE `jadwalkuliah` (
  `IDJADWALKULIAH` int(11) NOT NULL,
  `IDIDENTITTAS` int(11) DEFAULT NULL,
  `IDMATAKULIAH` int(11) DEFAULT NULL,
  `IDJADWAL` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `matakuliah`
--

CREATE TABLE `matakuliah` (
  `IDMATAKULIAH` int(11) NOT NULL,
  `NAMAMATAKULIAH` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `rule`
--

CREATE TABLE `rule` (
  `IDRULE` int(11) NOT NULL,
  `NAMERULE` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `fingerprint`
--
ALTER TABLE `fingerprint`
  ADD PRIMARY KEY (`IDFINGERPRINT`),
  ADD KEY `FK_FINGERPR_RELATIONS_JADWAL` (`IDJADWAL`),
  ADD KEY `FK_FINGERPR_RELATIONS_IDENTITA` (`IDIDENTITTAS`);

--
-- Indexes for table `hystory`
--
ALTER TABLE `hystory`
  ADD PRIMARY KEY (`IDHYSTORY`);

--
-- Indexes for table `identitas`
--
ALTER TABLE `identitas`
  ADD PRIMARY KEY (`IDIDENTITTAS`),
  ADD KEY `FK_IDENTITA_RELATIONS_RULE` (`IDRULE`);

--
-- Indexes for table `jadwal`
--
ALTER TABLE `jadwal`
  ADD PRIMARY KEY (`IDJADWAL`);

--
-- Indexes for table `jadwalkuliah`
--
ALTER TABLE `jadwalkuliah`
  ADD PRIMARY KEY (`IDJADWALKULIAH`),
  ADD KEY `FK_JADWALKU_RELATIONS_IDENTITA` (`IDIDENTITTAS`),
  ADD KEY `FK_JADWALKU_RELATIONS_JADWAL` (`IDJADWAL`),
  ADD KEY `FK_JADWALKU_RELATIONS_MATAKULI` (`IDMATAKULIAH`);

--
-- Indexes for table `matakuliah`
--
ALTER TABLE `matakuliah`
  ADD PRIMARY KEY (`IDMATAKULIAH`);

--
-- Indexes for table `rule`
--
ALTER TABLE `rule`
  ADD PRIMARY KEY (`IDRULE`);

--
-- Constraints for dumped tables
--

--
-- Constraints for table `fingerprint`
--
ALTER TABLE `fingerprint`
  ADD CONSTRAINT `FK_FINGERPR_RELATIONS_IDENTITA` FOREIGN KEY (`IDIDENTITTAS`) REFERENCES `identitas` (`IDIDENTITTAS`),
  ADD CONSTRAINT `FK_FINGERPR_RELATIONS_JADWAL` FOREIGN KEY (`IDJADWAL`) REFERENCES `jadwal` (`IDJADWAL`);

--
-- Constraints for table `identitas`
--
ALTER TABLE `identitas`
  ADD CONSTRAINT `FK_IDENTITA_RELATIONS_RULE` FOREIGN KEY (`IDRULE`) REFERENCES `rule` (`IDRULE`);

--
-- Constraints for table `jadwalkuliah`
--
ALTER TABLE `jadwalkuliah`
  ADD CONSTRAINT `FK_JADWALKU_RELATIONS_IDENTITA` FOREIGN KEY (`IDIDENTITTAS`) REFERENCES `identitas` (`IDIDENTITTAS`),
  ADD CONSTRAINT `FK_JADWALKU_RELATIONS_JADWAL` FOREIGN KEY (`IDJADWAL`) REFERENCES `jadwal` (`IDJADWAL`),
  ADD CONSTRAINT `FK_JADWALKU_RELATIONS_MATAKULI` FOREIGN KEY (`IDMATAKULIAH`) REFERENCES `matakuliah` (`IDMATAKULIAH`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
