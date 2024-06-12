package com.advatek.stas.network

import com.advatek.stas.model.Scan
import com.advatek.stas.model.ScanIN
import com.advatek.stas.model.ScanOut
import retrofit2.Call
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.GET
import retrofit2.http.Headers
import retrofit2.http.POST
import retrofit2.http.Path

interface RestApi {

    @Headers("Content-Type: application/json")
    @POST("api/scan")
    fun addScan(@Body scan: ScanIN): Call<ScanOut>

    @GET("api/scan/getLastScanByEmployeeId/{employeeId}")
    fun getLastScanByEmployeeId(@Path("employeeId") employeeId: Int): Call<Scan>
}
