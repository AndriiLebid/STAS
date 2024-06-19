package com.advatek.stas.Datasource

// Create the Room database

import androidx.room.Database
import androidx.room.RoomDatabase

@Database(entities = [ScanEntity::class], version = 1)
abstract class AppDatabase : RoomDatabase() {
    abstract fun scanDao(): ScanDao
}
