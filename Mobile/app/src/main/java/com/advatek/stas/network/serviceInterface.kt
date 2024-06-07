package com.advatek.stas.network

import com.advatek.stas.model.Scan
import com.advatek.stas.model.ScanOut
import retrofit2.Call
import retrofit2.http.Body
import retrofit2.http.Headers
import retrofit2.http.POST

interface RestApi {

    @Headers("Content-Type: application/json")
    @POST("api/scan")
    fun addScan(@Body scan: Scan): Call<ScanOut>
}
