﻿USING PERIODIC COMMIT
LOAD CSV WITH HEADERS FROM 'file:///C:/Users/Ricky/Downloads/NaPTANcsv/Stops.csv' AS line
CREATE (:NaptanStop {
  Type: "Naptan",
  AtcoCode: line.AtcoCode,
  Name: line.CommonName,
  Street: line.Street,
  Latitude: toFloat(line.Latitude),
  Longitude: toFloat(line.Longitude),
  NatGazId: line.NptgLocalityCode})

  CREATE INDEX ON :NaptanStop(AtcoCode)
  CREATE INDEX ON :NaptanStop(NatGazId)