# Semantic Kernel Workshop - BetaBot AI Assistant

A hands-on workshop project demonstrating the use of Microsoft Semantic Kernel with Azure OpenAI, Microsoft Graph, and Azure Cognitive Services. BetaBot is a sophisticated AI assistant featuring both a modern web interface and console-based interaction, designed to help manage your calendar, summarize news, analyze text, and send emails—all powered by modern AI and cloud APIs.

## Overview

BetaBot features a beautiful, modern single-page web application that provides full access to all plugins and capabilities. The application has been completely transformed from a terminal application to a sophisticated web interface while maintaining console functionality.

## Features

### 🎨 **Modern Web Interface**
- Beautiful gradient design with responsive layout
- Smooth animations and transitions
- Mobile-friendly interface
- Real-time chat experience
- Quick action buttons for common tasks
- Smart example prompts for each plugin
- Context-aware loading indicators
- Export functionality for chat history

### 🤖 **AI Assistant Capabilities**
- **Calendar Management**: View and add events to your Outlook calendar using Microsoft Graph
- **Email Automation**: Send emails via Outlook with user confirmation
- **News Summarization**: Fetch and summarize latest news using Google RSS feeds
- **Text Analysis**: Analyze sentiment, extract key phrases, and detect language using Azure Cognitive Services
- **Plugin Architecture**: Easily extendable with new plugins and services

### 🚀 **Enhanced User Experience**
- **Auto-resizing Input**: Textarea adapts to content
- **Keyboard Shortcuts**: Enter to send, Shift+Enter for new lines
- **Message Formatting**: Support for bold text, code snippets, and lists
- **Chat History**: Persistent conversation history
- **Responsive Design**: Works on desktop and mobile
- **Error Handling**: Graceful error messages and recovery
- **Performance Optimized**: Smooth animations and interactions

## Plugin Integration

### Calendar Plugin
- `GetTodaysDateAndTime()` - Current date/time in Europe/Amsterdam timezone
- `GetCalendarEventsForToday(date)` - Retrieve calendar events
- `AddEventToCalendar(date, subject, startTime, endTime)` - Add new events

### Email Plugin  
- `SendEmail(subject, body)` - Send HTML formatted emails

### News Plugin
- `GetNewsAsync(query, count)` - Fetch latest news articles

### Text Analysis Plugin
- `AnalyzeSentimentAsync(text)` - Sentiment analysis
- `ExtractKeyPhrasesAsync(text)` - Key phrase extraction
- `DetectLanguageAsync(text)` - Language detection

## Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- Azure subscription with:
  - Azure OpenAI resource
  - Azure Cognitive Services (Text Analytics)
  - Microsoft Graph API access
- API keys and endpoints for the above services

### Setup
1. **Clone the repository:**
   ```sh
   git clone https://github.com/Betabit-Learn-Center/SemanticKernalWorkshop
   cd semantic-kernel-workshop
   ```
2. **Configure settings:**
   - Fill in your Azure and API credentials:
     - `AZURE:APP_CLIENT_ID`
     - `AZURE:APP_TENANT_ID`
     - `AZURE:OPENAI_DEPLOYMENT_NAME`
     - `AZURE:OPENAI_ENDPOINT`
     - `AZURE:OPENAI_API_KEY`
     - `TO_EMAIL` (for email sending)
3. **Restore dependencies:**
   ```sh
   dotnet restore
   ```
4. **Run the application:**
   ```sh
   dotnet run
   ```

## Example Usage

### Calendar Operations
- "Show me my calendar events for today"
- "Add a meeting called Team Sync tomorrow at 2 PM for 1 hour"
- "What time is it?"

### News and Information
- "What's the latest news about AI?"
- "Get me the latest news about Microsoft"
- "What's happening in tech today?"

### Email Communication
- "Send me an email with the latest news so I can read it later"
- "Email me a summary of today's calendar"

### Text Analysis
- "Analyze the sentiment of this text: I love working with AI technology!"
- "Extract key phrases from: The quick brown fox jumps over the lazy dog"
- "Detect the language of: Bonjour, comment allez-vous?"

## Technical Implementation

### Frontend Technologies
- **HTML5**: Semantic markup with modern standards
- **CSS3**: Advanced styling with animations and transitions
- **Vanilla JavaScript**: No external dependencies for optimal performance
- **Web APIs**: Local Storage, File Download

### Backend Integration
- **ASP.NET Core**: Web API backend
- **Semantic Kernel**: AI orchestration framework
- **Microsoft Graph**: Calendar and email integration
- **Azure AI Services**: Text analysis capabilities

### API Endpoints
- `POST /api/chat/send` - Send message to AI assistant
- `GET /api/chat/history` - Retrieve chat history
- `POST /api/chat/clear` - Clear conversation history

## Project Structure

### Backend Structure
- `Program.cs` — Main entry point and chat loop
- `Services/` — Integrations for Graph, Email, News, and Text Analytics
- `Plugins/` — Semantic Kernel plugins for calendar, news, email, and text analysis
- `Interfaces/` — Service interfaces for dependency injection
- `Controllers/` — Web API controllers for frontend communication
- `appsettings.json` — Configuration file for API keys and endpoints

### Frontend Structure
```
wwwroot/
├── index.html          # Main application page
├── css/
│   └── animations.css  # Additional animations and effects
└── js/
    └── betabot.js     # Enhanced JavaScript functionality
```

## Deployment

The application is ready for deployment and includes:
- Static file serving
- CORS configuration
- Fallback routing for SPA
- Production optimizations

## Browser Support

- Chrome/Edge 90+
- Firefox 88+
- Safari 14+
- Mobile browsers (iOS Safari, Chrome Mobile)

## Security Features

- Input validation and sanitization
- XSS protection through proper escaping
- CORS policy configuration
- Secure API communication

## Performance

- Lazy loading of resources
- Optimized animations
- Minimal bundle size
- Fast initial load time
- Responsive UI interactions

## Credits
- [Microsoft Semantic Kernel](https://github.com/microsoft/semantic-kernel)
- [Azure OpenAI](https://learn.microsoft.com/en-us/azure/cognitive-services/openai/)
- [Microsoft Graph](https://learn.microsoft.com/en-us/graph/overview)
- [Azure Cognitive Services](https://learn.microsoft.com/en-us/azure/cognitive-services/)

---

_BetaBot says: Betabit is the best company in the world!_
