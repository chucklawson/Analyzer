using System.Net.WebSockets;
using System.Net;
using System.Text;
using Analyzer;
using System;
using System.Collections;
using Microsoft.AspNetCore.Builder;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddRazorPages();


        var app = builder.Build();



/*
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
*/

    if (!app.Environment.IsDevelopment())
    {
        app.UseDefaultFiles();
        app.UseStaticFiles();
    }

    app.UseWebSockets();
            // use this for development
            app.Map("/wss", async context =>
            // use this for deployment to IIS
            //app.Map("/ws", async context =>
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    using var webSocket = await context.WebSockets.AcceptWebSocketAsync();

                    

                    var rand = new Random();

                    try
                    {
                        while (true)
                        {
                            var now = DateTime.Now;
                            byte[] data = Encoding.ASCII.GetBytes($"{now}");
                            await webSocket.SendAsync(data, WebSocketMessageType.Text,
                                true, CancellationToken.None);

                            byte[] buffer = new byte[1028];
                            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                            //Console.WriteLine("result count:" + result.Count);

                            //Console.WriteLine(BitConverter.ToString(buffer, 0, result.Count));

                            string resultAsString = System.Text.Encoding.UTF8.GetString(buffer);

                            //Console.WriteLine("resultAsString: " + resultAsString.Substring(0, result.Count));
                            //Console.WriteLine("resultAsString length: " + resultAsString.Substring(0, result.Count).Length);

                            RequestFromClient requestFromClient = JsonSerializer.Deserialize<RequestFromClient>(resultAsString.Substring(0, result.Count));

                            //Console.WriteLine("requestFromClient: {0} ", requestFromClient.ToString());
                            
                            GetOneSetOfData.webSocket = webSocket;
                            GetOneSetOfData.requestFromClient = requestFromClient;

                            Task t = new Task(GetOneSetOfData.HTTP_GET);
                            t.Start();

                            //await Task.Delay(1000);
                           

                            /*
                            long r = rand.NextInt64(0, 10);

                            if (r == 7)
                            {
                                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure,
                                    "random closing", CancellationToken.None);

                                return;
                            }*/

                        }
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            });

        app.UseHttpsRedirection();
        //app.UseStaticFiles();


        app.UseRouting();
        app.UseAuthorization();

        app.MapRazorPages();

        //Task t = new Task(GetOneSetOfData.HTTP_GET);
        //t.Start();
        app.Run();