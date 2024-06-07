package com.advatek.stas.model

import kotlinx.serialization.Serializable
import java.time.LocalDateTime

@Serializable
data class Scan(
    val cardNumber: String,
    val scanDate: LocalDateTime,
    val scanType: String
)

