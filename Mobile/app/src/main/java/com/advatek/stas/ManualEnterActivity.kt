package com.advatek.stas

import android.app.DatePickerDialog
import android.app.TimePickerDialog
import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.compose.foundation.clickable
import androidx.compose.material.*
import androidx.compose.material3.Button
import androidx.compose.material3.Text
import androidx.compose.material3.TextField
import androidx.compose.runtime.Composable
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.ButtonDefaults
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedTextField
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.text.input.TextFieldValue
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import java.text.SimpleDateFormat
import java.util.Calendar
import java.util.Locale

class ManualEnterActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            ManualEnter()
        }
    }
}

@Composable
fun DateTimePickerField(dateTime: String, onDateTimeChange: (String) -> Unit) {
    val context = LocalContext.current

    val calendar = Calendar.getInstance()
    val year = calendar.get(Calendar.YEAR)
    val month = calendar.get(Calendar.MONTH)
    val day = calendar.get(Calendar.DAY_OF_MONTH)
    val hour = calendar.get(Calendar.HOUR_OF_DAY)
    val minute = calendar.get(Calendar.MINUTE)

    val datePickerDialog = DatePickerDialog(
        context,
        { _, selectedYear, selectedMonth, selectedDay ->
            val timePickerDialog = TimePickerDialog(
                context,
                { _, selectedHour, selectedMinute ->
                    val selectedDateTime = Calendar.getInstance().apply {
                        set(selectedYear, selectedMonth, selectedDay, selectedHour, selectedMinute)
                    }
                    val formattedDateTime =
                        SimpleDateFormat("dd/MM/yyyy HH:mm", Locale.getDefault()).format(
                            selectedDateTime.time
                        )
                    onDateTimeChange(formattedDateTime)
                },
                hour,
                minute,
                true
            )
            timePickerDialog.show()
        },
        year,
        month,
        day
    )

    OutlinedTextField(
        value = dateTime,
        onValueChange = { onDateTimeChange(it) },
        label = { Text("Enter DateTime") },
        modifier = Modifier
            .fillMaxWidth()
            .clickable { datePickerDialog.show() },
        readOnly = true
    )
}

@Composable
fun ManualEnter(modifier: Modifier = Modifier) {
    var scanCode by remember { mutableStateOf("") }
    val context = LocalContext.current
    val inButtonColors = ButtonDefaults.buttonColors(containerColor = Color.Blue)
    val outButtonColors = ButtonDefaults.buttonColors(containerColor = Color.Red)

    var dateTime by remember { mutableStateOf("") }

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
            DateTimePickerField(dateTime = dateTime, onDateTimeChange = { dateTime = it })
            Spacer(modifier = Modifier.height(16.dp))
            Row(
                horizontalArrangement = Arrangement.spacedBy(96.dp),
                modifier = Modifier.fillMaxWidth()
            ) {
                Column(
                    modifier = Modifier.weight(1f)
                ) {
                    Button(
                        onClick = { handleInAction(context, scanCode); scanCode = "" },
                        modifier = Modifier.fillMaxWidth(),
                        colors = inButtonColors,
                        shape = RoundedCornerShape(topEnd = 16.dp, bottomEnd = 16.dp)
                    ) {
                        Text("In")
                    }
                    Spacer(modifier = Modifier.height(8.dp))
                    Button(
                        onClick = { handleOutAction(context, scanCode); scanCode = "" },
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
                }
            }
            Spacer(modifier = Modifier.height(48.dp))
            Row(
                horizontalArrangement = Arrangement.SpaceEvenly,
                modifier = Modifier.fillMaxWidth()
            ) {
                Button(onClick = { scanCode = "" }) {
                    Text("Back To Main")
                }
            }
        }
    }
}
