package com.advatek.stas.model

import com.google.gson.annotations.SerializedName
import kotlinx.serialization.Serializable
import java.time.LocalDateTime

@Serializable
data class Scan(
    @SerializedName("scanId") val scanId: Int,
    @SerializedName("employeeId") val employeeId: Int,
    @SerializedName("scanDate") val scanDate: String,
    @SerializedName("scanType") val scanType: Int,
    @SerializedName("recordVersion") val recordVersion: String
)


