package com.advatek.stas.model

import com.google.gson.annotations.SerializedName
import kotlinx.serialization.Serializable
import java.time.LocalDateTime

@Serializable
data class ScanIN(
    @SerializedName("employeeCardNumber") val employeeCardNumber: String,
    @SerializedName("scanDate") val scanDate: String,
    @SerializedName("scanType") val scanType: String
)


