package com.advatek.stas.Datasource

// Create the Room entities:

import androidx.room.Entity
import androidx.room.PrimaryKey

@Entity(tableName = "scans")
data class ScanEntity(
    @PrimaryKey(autoGenerate = true) val id: Int = 0,
    val employeeCardNumber: String,
    val scanDate: String,
    val scanType: String
)
