package com.advatek.stas.model

import kotlinx.serialization.Serializable
import java.time.LocalDateTime

@Serializable
data class ScanOut(

    val scanId: Int,
    val employeeId: Int,
    val scanDate: LocalDateTime,
    val scanType: Int
)
