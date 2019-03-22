select HdNumber, PackStationName, ScanTimestamp from hd_scan 
inner join hd on hd.HdID = hd_scan.HdID
inner join pack_station on pack_station.PackStationID = hd_scan.PackStationID
where HdNumber = '000000050002036999'