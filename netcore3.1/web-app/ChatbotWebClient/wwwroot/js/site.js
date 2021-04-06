// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function CloseChat() {
    document.getElementById("chatbox").style.display = "none";
    document.getElementById("chatbutton").style.display = "flex";
}

function OpenChat() {
    document.getElementById("chatbutton").style.display = "none";
    document.getElementById("chatbox").style.display = "block";

    const store = window.WebChat.createStore({}, ({ dispatch }) => next => action => {
        if (action.type === 'DIRECT_LINE/CONNECT_FULFILLED') {
            dispatch({
                type: 'WEB_CHAT/SEND_EVENT',
                payload: {
                    name: 'webchat/join',
                    value: {
                        language: window.navigator.language
                    }
                }
            });
        }
        return next(action);
    });

    window.WebChat.renderWebChat(
        {
            directLine: window.WebChat.createDirectLine({
                token: '@ViewData["DirectLineToken"]'
            }),
            store,
        },
        document.getElementById('chatbody')
    );
}
