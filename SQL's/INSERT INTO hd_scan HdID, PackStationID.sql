INSERT INTO `vas_productivity_database`.`hd_scan` (`HdID`,`PackStationID`) VALUES (
(select HdID from vas_productivity_database.hd where HdNumber = '000000050002049333'),
(select PackStationID from vas_productivity_database.pack_station where PackStationName = 'VPACK003')
);