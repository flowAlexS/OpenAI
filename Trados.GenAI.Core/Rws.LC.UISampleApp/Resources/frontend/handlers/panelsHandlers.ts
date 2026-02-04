import { trados, ExtensibilityEventDetail } from "@trados/trados-ui-extensibility";
import { logExtensionData } from "./helpers";

/**
 * Handles myCustomSidebarBox's render event.
 * Adds HTML content to the sidebarBox.
 *
 * @param detail The event detail object.
 */
export const myCustomSidebarBoxRendered = (
    detail: ExtensibilityEventDetail
) => {
    logExtensionData(detail);

    const sidebarBoxContentWrapper = document.getElementById(detail.domElementId);
    if (sidebarBoxContentWrapper) {
        // Reset content for re-renders: as the state changes in the Trados UI depending on user actions, re-renders occur.
        sidebarBoxContentWrapper.innerHTML = "";

        // Create and append div.
        const div = document.createElement("div");
        div.innerHTML = `Custom sidebar box content inserted on render.`;
        sidebarBoxContentWrapper.appendChild(div);
    }
};

/**
 * Handles myCustomPanel's render event.
 * Adds HTML content to the panel.
 *
 * @param detail The event detail object.
 */
export const myCustomPanelRendered = (
    detail: ExtensibilityEventDetail
) => {
    logExtensionData(detail);
    
    const panelContentWrapper = document.getElementById(detail.domElementId);
    if (panelContentWrapper) {
        // Reset content for re-renders: as the state changes in the Trados UI depending on user actions, re-renders occur.
        panelContentWrapper.innerHTML = "";

        // Create and append div.
        const div = document.createElement("div");
        div.innerHTML = `Custom panel content inserted on render.`;
        panelContentWrapper.appendChild(div);
    }
};

/**
 * Handles myCustomTab's render event.
 * Adds HTML content to the tab.
 *
 * @param detail The event detail object.
 */
export const myCustomTabRendered = (
    detail: ExtensibilityEventDetail
) => {
    logExtensionData(detail);

    const tabContentWrapper = document.getElementById(detail.domElementId);
    if (tabContentWrapper) {
        // Reset content for re-renders: as the state changes in the Trados UI depending on user actions, re-renders occur.
        tabContentWrapper.innerHTML = "";

        // Create and append div.
        const div = document.createElement("div");
        div.innerHTML = `Custom tab content inserted on render.`;
        tabContentWrapper.appendChild(div);
    }
};

class PromptItem {
    public llmProvider: string = "";
    public model: string = "";
    public sysPrompt: string = "";
    public userPrompt: string = "";
    public context: string = "";
    public translation: string = "";
    public createdAt: Date = new Date();

    constructor(
        llmProvider: string = "",
        model: string = "",
        sysPrompt: string = "",
        userPrompt: string = "",
        context: string = "",
        translation: string = "",
        createdAt: Date = new Date()
    ) {
        this.llmProvider = llmProvider;
        this.model = model;
        this.sysPrompt = sysPrompt;
        this.userPrompt = userPrompt;
        this.context = context;
        this.translation = translation;
        this.createdAt = createdAt;
    }
}

export const callPublicApiLLMNotificationTubRendered = (
    detail: ExtensibilityEventDetail
) => {
    const promptItems: PromptItem[] = [];

    logExtensionData(detail);
    trados.callAppApi({
        url: `api/repository/`,
        method: "GET"
    })
        .then(data => {
            console.log(data?.responseData)
            data?.responseData?.forEach((pi: any) => {
                const promptItem = new PromptItem(
                    pi.provider,
                    pi.model,
                    pi.systemInstructions,
                    pi.userPrompt,
                    pi.contextUri,
                    pi.translation,
                    pi.createdAt ? new Date(pi.createdAt) : undefined
                );

                promptItems.push(promptItem);
            });

            renderPromptItems(promptItems, detail.domElementId);
        })
        .catch(err => {
        });

    
}

function renderPromptItems(promptItems: PromptItem[], domElementId: string) {
    if (promptItems.length === 0) {
        const tabContentWrapper = document.getElementById(domElementId);
        if (!tabContentWrapper) return;

        tabContentWrapper.innerHTML = "<p>No Prompt Items Saved yet</p>";

        return;
    }

    const tabContentWrapper = document.getElementById(domElementId);
    if (!tabContentWrapper) return;

    tabContentWrapper.innerHTML = "";

    // Notification container
    var container = document.createElement("div");
    container.style.display = "flex";
    container.style.flexDirection = "column";
    container.style.gap = "12px";

    promptItems.forEach(function (item) {
        var notification = document.createElement("div");
        notification.style.border = "1px solid #ccc";
        notification.style.borderRadius = "8px";
        notification.style.padding = "12px";
        notification.style.backgroundColor = "#f9f9f9";
        notification.style.display = "flex";
        notification.style.gap = "12px";
        notification.style.alignItems = "flex-start";

        // Text content
        var textContent = document.createElement("div");
        textContent.style.display = "flex";
        textContent.style.flexDirection = "column";
        textContent.style.gap = "4px";

        var providerEl = document.createElement("div");
        providerEl.innerHTML = "<strong>LLM Provider:</strong> " + item.llmProvider;

        var modelEl = document.createElement("div");
        modelEl.innerHTML = "<strong>Model:</strong> " + item.model;

        var sysPromptEl = document.createElement("div");
        sysPromptEl.innerHTML = "<strong>System Prompt:</strong> " + item.sysPrompt;

        var userPromptEl = document.createElement("div");
        userPromptEl.innerHTML = "<strong>User Prompt:</strong> " + item.userPrompt;

        var translationEl = document.createElement("div");
        translationEl.innerHTML = "<strong>Translation:</strong> " + item.translation;

        var createdAtEl = document.createElement("div");
        createdAtEl.innerHTML = "<strong>Created At:</strong> " + formatDate(item.createdAt);

        var contextEl = document.createElement("div");
        contextEl.innerHTML = '<strong>Context URL:</strong> <a href="' + item.context + '" target="_blank">' + item.context + "</a>";

        textContent.appendChild(providerEl);
        textContent.appendChild(modelEl);
        textContent.appendChild(sysPromptEl);
        textContent.appendChild(userPromptEl);
        textContent.appendChild(translationEl);
        textContent.appendChild(createdAtEl);
        textContent.appendChild(contextEl);

        // Preview (image or iframe)
        var previewWidth = 250; // bigger for better visibility
        var previewHeight = 200;

        var preview = document.createElement("img");
        preview.style.width = previewWidth + "px";
        preview.style.height = previewHeight + "px";
        preview.style.objectFit = "contain";
        preview.style.borderRadius = "4px";
        preview.alt = "Context preview";

        notification.appendChild(textContent);
        container.appendChild(notification);
    });

    tabContentWrapper.appendChild(container);
}

function pad(num: number, size: number = 2) {
    let s = num.toString();
    while (s.length < size) s = "0" + s;
    return s;
}

function formatDate(d: Date) {
    const yyyy = d.getFullYear();
    const mm = pad(d.getMonth() + 1);
    const dd = pad(d.getDate());
    const hh = pad(d.getHours());
    const min = pad(d.getMinutes());

    return `${yyyy} ${mm}-${dd} ${hh}:${min}`;
}
