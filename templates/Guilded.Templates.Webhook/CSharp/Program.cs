using Guilded.Webhook;

var webhookClient = new GuildedWebhookClient("...url here...");

await webhookClient.CreateMessageAsync("Guilded.NET webhook client works!");