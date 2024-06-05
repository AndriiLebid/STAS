package com.advatek.stas

import android.os.Bundle
import android.widget.TextClock
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.enableEdgeToEdge
import androidx.compose.foundation.layout.*
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.compose.ui.viewinterop.AndroidView
import androidx.navigation.NavHost
import androidx.navigation.NavHostController
import com.advatek.stas.ui.theme.STASTheme

class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContent {
            STASTheme {
                Scaffold(modifier = Modifier.fillMaxSize()) { innerPadding ->
                    ScanCodeForm(modifier = Modifier.padding(innerPadding))
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
fun ScanCodeForm(modifier: Modifier = Modifier) {
    var scanCode by remember { mutableStateOf("") }

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
                modifier = Modifier.fillMaxWidth()
            )
            Spacer(modifier = Modifier.height(16.dp))
            Row(
                horizontalArrangement = Arrangement.SpaceEvenly,
                modifier = Modifier.fillMaxWidth()
            ) {
                Button(onClick = { /* Handle In action */ }) {
                    Text("In")
                }
                Button(onClick = { /* Handle Out action */ }) {
                    Text("Out")
                }
            }
        }
    }
}



@Preview(showBackground = true)
@Composable
fun ScanCodeFormPreview() {
    STASTheme {
        ScanCodeForm()
    }
}

