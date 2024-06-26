package com.advatek.stas

import android.content.Context
import android.os.Bundle
import android.widget.TextClock
import android.widget.Toast
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.enableEdgeToEdge
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.compose.ui.viewinterop.AndroidView
import com.advatek.stas.model.ScanIN
import com.advatek.stas.network.RestApiService
import com.advatek.stas.ui.theme.STASTheme
import java.time.LocalDateTime
import android.content.Intent
import androidx.compose.foundation.text.KeyboardOptions
import androidx.compose.ui.text.input.KeyboardType
import androidx.room.Room
import com.advatek.stas.Datasource.AppDatabase
import com.advatek.stas.Datasource.ScanEntity
import com.advatek.stas.network.connectionCheck
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch

class MainActivity : ComponentActivity() {
    private lateinit var db: AppDatabase
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        db = Room.databaseBuilder(applicationContext, AppDatabase::class.java, "stas-db").build()

        val sharedPreferences = getSharedPreferences("app_prefs", Context.MODE_PRIVATE)
        val token = sharedPreferences.getString("jwt_token", null)


        //syncLocalData(this, db)
        setContent {
            STASTheme {
//                Scaffold(modifier = Modifier.fillMaxSize()) { innerPadding ->
//                    ScanCodeForm(modifier = Modifier.padding(innerPadding), db)
//                }

                if (token == null) {
                    LoginScreen(onLoginSuccess = {
                        recreate() // Restart activity to load the main content
                    })
                } else {
                    Scaffold(modifier = Modifier.fillMaxSize()) { innerPadding ->
                        ScanCodeForm(modifier = Modifier.padding(innerPadding), db)
                    }
                }

            }
        }
    }
}

@Composable
fun RealTimeClock() {
    AndroidView(
        factory = { context ->
            TextClock(context).apply {
                format12Hour = "hh:mm:ss a"
                textSize = 24f
            }
        },
        modifier = Modifier.padding(16.dp)
    )
}

@Composable
fun ScanCodeForm(modifier: Modifier = Modifier, db: AppDatabase) {
    var scanCode by remember { mutableStateOf("") }
    val context = LocalContext.current

    val inButtonColors = ButtonDefaults.buttonColors(containerColor = Color.Blue)
    val outButtonColors = ButtonDefaults.buttonColors(containerColor = Color.Red)
    val grayButtonColors = ButtonDefaults.buttonColors(containerColor = Color.Gray)

    Box(
        modifier = modifier
            .fillMaxSize()
            .padding(16.dp),
        contentAlignment = Alignment.Center
    ) {
        Column(
            horizontalAlignment = Alignment.CenterHorizontally
        ) {
            RealTimeClock()
            Spacer(modifier = Modifier.height(96.dp))
            Text(text = "Scan Code", style = MaterialTheme.typography.titleLarge)
            Spacer(modifier = Modifier.height(8.dp))
            OutlinedTextField(
                value = scanCode,
                onValueChange = { scanCode = it },
                label = { Text("Enter code") },
                keyboardOptions = KeyboardOptions(keyboardType = KeyboardType.Number),
                modifier = Modifier.fillMaxWidth()
            )
            Spacer(modifier = Modifier.height(16.dp))
            Row(
                horizontalArrangement = Arrangement.spacedBy(96.dp),
                modifier = Modifier.fillMaxWidth()
            ) {
                Column(
                    modifier = Modifier.weight(1f)
                ) {
                    Button(
                        onClick = { handleInAction(context, scanCode, db); scanCode = "" },
                        modifier = Modifier.fillMaxWidth(),
                        colors = inButtonColors,
                        shape = RoundedCornerShape(topEnd = 16.dp, bottomEnd = 16.dp)
                    ) {
                        Text("In")
                    }
                    Spacer(modifier = Modifier.height(8.dp))
                    Button(
                        onClick = { handleOutAction(context, scanCode, db); scanCode = "" },
                        modifier = Modifier.fillMaxWidth(),
                        colors = outButtonColors,
                        shape = RoundedCornerShape(topEnd = 16.dp, bottomEnd = 16.dp)
                    ) {
                        Text("Out")
                    }
                }
                Column(
                    modifier = Modifier.weight(1f)
                ) {
                    Button(
                        onClick = { handleBreakInAction(context, scanCode, db); scanCode = "" },
                        modifier = Modifier.fillMaxWidth(),
                        shape = RoundedCornerShape(topStart = 16.dp, bottomStart = 16.dp)
                    ) {
                        Text("Break Start")
                    }
                    Spacer(modifier = Modifier.height(8.dp))
                    Button(
                        onClick = { handleBreakOutAction(context, scanCode, db); scanCode = "" },
                        modifier = Modifier.fillMaxWidth(),
                        colors = grayButtonColors,
                        shape = RoundedCornerShape(topStart = 16.dp, bottomStart = 16.dp)

                    ) {
                        Text("Break End")
                    }
                    Spacer(modifier = Modifier.height(8.dp))
                    Button(
                        onClick = { handleLunchInAction(context, scanCode, db); scanCode = "" },
                        modifier = Modifier.fillMaxWidth(),
                        shape = RoundedCornerShape(topStart = 16.dp, bottomStart = 16.dp)
                    ) {
                        Text("Lunch Start")
                    }
                    Spacer(modifier = Modifier.height(8.dp))
                    Button(
                        onClick = { handleLunchOutAction(context, scanCode, db); scanCode = "" },
                        modifier = Modifier.fillMaxWidth(),
                        colors = grayButtonColors,
                        shape = RoundedCornerShape(topStart = 16.dp, bottomStart = 16.dp)
                    ) {
                        Text("Lunch End")
                    }
                }
            }
            Spacer(modifier = Modifier.height(48.dp))
            Row(
                horizontalArrangement = Arrangement.SpaceEvenly,
                modifier = Modifier.fillMaxWidth()
            ) {
                Button(onClick = { scanCode = "" }) {
                    Text("Cancel")
                }
                Button(onClick = { handleLogout(context) }) {
                    Text("Logout")
                }
            }
        }
    }
}

fun handleLogout(context: Context) {
    val sharedPreferences = context.getSharedPreferences("app_prefs", Context.MODE_PRIVATE)
    with(sharedPreferences.edit()) {
        remove("jwt_token")
        apply()
    }
    val intent = Intent(context, MainActivity::class.java)
    intent.flags = Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_CLEAR_TASK
    context.startActivity(intent)
}


fun handleAction(context: Context, scanCode: String, scanType: String, db: AppDatabase) {

    if (!connectionCheck(context)) {
        Toast.makeText(context, "No internet connection available", Toast.LENGTH_LONG).show()
        val scan = ScanEntity(
            employeeCardNumber = scanCode,
            scanDate = LocalDateTime.now().toString(),
            scanType = scanType
        )
        CoroutineScope(Dispatchers.IO).launch {
            db.scanDao().insert(scan)
        }
        Toast.makeText(context, "The scan is saved locally.", Toast.LENGTH_LONG).show()
        return
    }

    syncLocalData(context, db)

    val apiService = RestApiService(context)

    val scan = ScanIN(
        employeeCardNumber = scanCode,
        scanDate = LocalDateTime.now().toString(),
        scanType = scanType
    )

    apiService.addScan(scan) {
        if (it?.scanId != null) {
            Toast.makeText(context, "Scan added successfully with ID: ${it.scanId}", Toast.LENGTH_LONG).show()
        } else {
            if (!scanCode.isNullOrEmpty()) {
                val intent = Intent(context, ManualEnterActivity::class.java)
                intent.putExtra("scanCode", scanCode)
                context.startActivity(intent)
            }
        }
    }


}


fun handleInAction(context: Context, scanCode: String, db: AppDatabase) {
    handleAction(context, scanCode, "IN", db)
}

fun handleOutAction(context: Context, scanCode: String, db: AppDatabase) {
    handleAction(context, scanCode, "OUT", db)
}

fun handleBreakInAction(context: Context, scanCode: String, db: AppDatabase) {
    handleAction(context, scanCode, "BreakStart", db)
}
fun handleBreakOutAction(context: Context, scanCode: String, db: AppDatabase) {
    handleAction(context, scanCode, "BreakEnd", db)
}
fun handleLunchInAction(context: Context, scanCode: String, db: AppDatabase) {
    handleAction(context, scanCode, "LunchStart", db)
}
fun handleLunchOutAction(context: Context, scanCode: String, db: AppDatabase) {
    handleAction(context, scanCode, "LunchEnd", db)
}


//fun handleGetLastScan(employeeId: Int) {
//    val apiService = RestApiService()
//    Log.d("handleGetLastScan", "Requesting last scan for employeeId: $employeeId")
//    apiService.getLastScanByEmployeeId(employeeId) { scan ->
//        if (scan != null) {
//            // Handle successful response
//            Log.d("handleGetLastScan", "Received scan: $scan")
//            println("Scan ID: ${scan.scanId}, Employee ID: ${scan.employeeId}")
//        } else {
//            // Handle error
//            Log.e("handleGetLastScan", "Error retrieving scan")
//            println("Error retrieving scan")
//        }
//    }
//}


fun syncLocalData(context: Context, db: AppDatabase) {
    if (connectionCheck(context)) {
        CoroutineScope(Dispatchers.IO).launch {
            val scans = db.scanDao().getAllScans()
            val apiService = RestApiService(context)
            scans.forEach { scan ->
                val scanIN = ScanIN(
                    employeeCardNumber = scan.employeeCardNumber,
                    scanDate = scan.scanDate,
                    scanType = scan.scanType
                )
                apiService.addScan(scanIN) {
                }
            }
            db.scanDao().deleteAll()
        }
    }
}



@Preview(showBackground = true)
@Composable
fun ScanCodeFormPreview() {
    val context = LocalContext.current
    val db = Room.inMemoryDatabaseBuilder(context, AppDatabase::class.java).build()
    STASTheme {
        ScanCodeForm(db = db)
    }
}


