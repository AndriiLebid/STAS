package com.advatek.stas.model

import java.time.LocalDateTime

class Scan (
    val cardNumber : String,
    val scanDate : LocalDateTime,
    val scanType: String
)