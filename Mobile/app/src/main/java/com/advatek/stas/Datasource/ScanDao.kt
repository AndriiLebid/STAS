package com.advatek.stas.Datasource

// Create DAO:
import androidx.room.Dao
import androidx.room.Insert
import androidx.room.Query

@Dao
interface ScanDao {
    @Insert
    fun insert(scan: ScanEntity)

    @Query("SELECT * FROM scans")
    fun getAllScans(): List<ScanEntity>

    @Query("DELETE FROM scans")
    fun deleteAll()

    @Query("DELETE FROM scans WHERE id = :scanId")
    fun deleteById(scanId: Int)
}
