// CodeMirror Editor Loader for 4kampf
// Uses CodeMirror 5 for code editing (GLSL and YAML)
// Note: API name "monacoLoader" kept for backward compatibility with C# service

window.monacoLoader = {
    editors: new Map(),
    loaded: false,
    debounceTimers: {},
    
    // Initialize CodeMirror
    init: async function() {
        if (this.loaded) return Promise.resolve();
        
        return new Promise((resolve, reject) => {
            // Check if CodeMirror is already loaded
            if (typeof CodeMirror !== 'undefined') {
                this.loaded = true;
                resolve();
                return;
            }
            
            // Load CodeMirror 5 from CDN (simpler than CodeMirror 6 for this use case)
            const script = document.createElement('script');
            script.src = 'https://cdn.jsdelivr.net/npm/codemirror@5.65.16/lib/codemirror.js';
            script.onload = () => {
                // Load CSS
                const link = document.createElement('link');
                link.rel = 'stylesheet';
                link.href = 'https://cdn.jsdelivr.net/npm/codemirror@5.65.16/lib/codemirror.css';
                document.head.appendChild(link);
                
                // Load dark theme
                const themeLink = document.createElement('link');
                themeLink.rel = 'stylesheet';
                themeLink.href = 'https://cdn.jsdelivr.net/npm/codemirror@5.65.16/theme/monokai.css';
                document.head.appendChild(themeLink);
                
                // Load language modes
                const languages = [
                    'https://cdn.jsdelivr.net/npm/codemirror@5.65.16/mode/glsl/glsl.js',
                    'https://cdn.jsdelivr.net/npm/codemirror@5.65.16/mode/yaml/yaml.js'
                ];
                
                let loaded = 0;
                languages.forEach((src) => {
                    const langScript = document.createElement('script');
                    langScript.src = src;
                    langScript.onload = () => {
                        loaded++;
                        if (loaded === languages.length) {
                            this.loaded = true;
                            resolve();
                        }
                    };
                    langScript.onerror = () => {
                        console.warn('Failed to load language mode:', src);
                        loaded++;
                        if (loaded === languages.length) {
                            this.loaded = true;
                            resolve(); // Continue even if some languages fail
                        }
                    };
                    document.head.appendChild(langScript);
                });
            };
            script.onerror = () => reject(new Error('Failed to load CodeMirror'));
            document.head.appendChild(script);
        });
    },
    
    // Get editor configuration options
    getEditorOptions: function(language, initialValue) {
        // Map language names to CodeMirror modes
        const langMap = {
            'glsl': 'glsl',
            'yaml': 'yaml',
            'yml': 'yaml'
        };
        
        const mappedLang = langMap[language] || 'glsl';
        
        return {
            value: initialValue || '',
            mode: mappedLang,
            theme: 'monokai',
            lineNumbers: true,
            lineWrapping: true,
            indentUnit: 4,
            tabSize: 4,
            readOnly: false,
            autofocus: false
        };
    },
    
    // Create editor
    createEditor: function(containerId, language, initialValue, callbackRef) {
        if (!this.loaded) {
            console.error('CodeMirror not loaded');
            return null;
        }
        
        const container = document.getElementById(containerId);
        if (!container) {
            console.error('Container not found:', containerId);
            return null;
        }
        
        // Check if editor already exists
        if (this.editors.has(containerId)) {
            console.warn('Editor already exists for container:', containerId, '- disposing first');
            this.dispose(containerId);
        }
        
        // Clear container
        container.innerHTML = '';
        
        // Get editor options
        const options = this.getEditorOptions(language, initialValue);
        
        // Create editor
        const editor = CodeMirror(container, options);
        
        // Set initial value
        if (initialValue) {
            editor.setValue(initialValue);
        }
        
        // Make editor fill container
        editor.setSize('100%', '100%');
        
        // Refresh editor to ensure proper rendering
        // Use multiple timeouts to catch different rendering states
        setTimeout(() => {
            editor.refresh();
        }, 50);
        setTimeout(() => {
            editor.refresh();
        }, 200);
        
        // Also refresh when container becomes visible (for hidden tabs)
        const containerObserver = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    // Container became visible, refresh editor
                    setTimeout(() => {
                        editor.refresh();
                    }, 50);
                }
            });
        }, { threshold: 0.1 });
        
        containerObserver.observe(container);
        
        // Store observer for cleanup
        if (!this.intersectionObservers) {
            this.intersectionObservers = new Map();
        }
        this.intersectionObservers.set(containerId, containerObserver);
        
        // Handle container resize - debounced to reduce CPU usage
        let resizeTimeout;
        const resizeObserver = new ResizeObserver(() => {
            clearTimeout(resizeTimeout);
            resizeTimeout = setTimeout(() => {
                editor.refresh();
            }, 100); // Debounce resize events
        });
        resizeObserver.observe(container);
        
        // Store resize observer for cleanup
        if (!this.resizeObservers) {
            this.resizeObservers = new Map();
        }
        this.resizeObservers.set(containerId, resizeObserver);
        
        // Add change listener if callback provided
        if (callbackRef) {
            editor.on('change', () => {
                clearTimeout(this.debounceTimers[containerId]);
                this.debounceTimers[containerId] = setTimeout(() => {
                    const value = editor.getValue();
                    callbackRef.invokeMethodAsync('OnChange', value);
                }, 500);
            });
        }
        
        // Store editor
        this.editors.set(containerId, editor);
        
        return editor;
    },
    
    setValue: function(containerId, value) {
        const editor = this.editors.get(containerId);
        if (editor) {
            editor.setValue(value || '');
        }
    },
    
    getValue: function(containerId) {
        const editor = this.editors.get(containerId);
        return editor ? editor.getValue() : '';
    },
    
    destroyEditor: function(containerId) {
        this.dispose(containerId);
    },
    
    dispose: function(containerId) {
        const editor = this.editors.get(containerId);
        if (editor) {
            // CodeMirror 5: get the wrapper element and remove it
            const wrapper = editor.getWrapperElement();
            if (wrapper && wrapper.parentNode) {
                wrapper.parentNode.removeChild(wrapper);
            }
            this.editors.delete(containerId);
        }
        
        // Disconnect resize observer
        if (this.resizeObservers && this.resizeObservers.has(containerId)) {
            const observer = this.resizeObservers.get(containerId);
            observer.disconnect();
            this.resizeObservers.delete(containerId);
        }
        
        // Disconnect intersection observer
        if (this.intersectionObservers && this.intersectionObservers.has(containerId)) {
            const observer = this.intersectionObservers.get(containerId);
            observer.disconnect();
            this.intersectionObservers.delete(containerId);
        }
        
        // Clear debounce timer
        if (this.debounceTimers[containerId]) {
            clearTimeout(this.debounceTimers[containerId]);
            delete this.debounceTimers[containerId];
        }
        
        // Clear container
        const container = document.getElementById(containerId);
        if (container) {
            container.innerHTML = '';
        }
    },
    
    undo: function(containerId) {
        const editor = this.editors.get(containerId);
        if (editor) {
            editor.undo();
        }
    },
    
    redo: function(containerId) {
        const editor = this.editors.get(containerId);
        if (editor) {
            editor.redo();
        }
    },
    
    canUndo: function(containerId) {
        const editor = this.editors.get(containerId);
        if (!editor) return false;
        return editor.historySize().undo > 0;
    },
    
    canRedo: function(containerId) {
        const editor = this.editors.get(containerId);
        if (!editor) return false;
        return editor.historySize().redo > 0;
    },
    
    showFind: function(containerId) {
        const editor = this.editors.get(containerId);
        if (editor) {
            // CodeMirror 5 doesn't have built-in search, but we can use a simple prompt
            const searchTerm = prompt('Find:');
            if (searchTerm) {
                editor.execCommand('find');
            }
        }
    },
    
    setLineNumbers: function(containerId, visible) {
        const editor = this.editors.get(containerId);
        if (editor) {
            editor.setOption('lineNumbers', visible);
        }
    },
    
    gotoLine: function(containerId, lineNumber) {
        const editor = this.editors.get(containerId);
        if (editor) {
            editor.setCursor(lineNumber - 1, 0);
            editor.scrollIntoView({ line: lineNumber - 1, ch: 0 });
            editor.focus();
        }
    },
    
    // Refresh editor (useful when container becomes visible or resizes)
    refresh: function(containerId) {
        const editor = this.editors.get(containerId);
        if (editor) {
            const container = document.getElementById(containerId);
            if (container) {
                // Check if container is visible
                const rect = container.getBoundingClientRect();
                const isVisible = rect.width > 0 && rect.height > 0 && 
                                 window.getComputedStyle(container).display !== 'none';
                
                if (isVisible) {
                    // Container is visible, refresh editor
                    editor.refresh();
                    // Also ensure size is correct
                    editor.setSize('100%', '100%');
                }
            } else {
                // Container doesn't exist, editor might have been destroyed
                console.warn('Container not found for editor:', containerId);
            }
        }
    },
    
    // Refresh all editors
    refreshAll: function() {
        this.editors.forEach((editor, containerId) => {
            this.refresh(containerId);
        });
    },
    
    // Insert text at cursor position
    insertText: function(containerId, text) {
        const editor = this.editors.get(containerId);
        if (editor) {
            const cursor = editor.getCursor();
            editor.replaceRange(text, cursor);
        }
    },
    
    // Replace selected text
    replaceSelection: function(containerId, text) {
        const editor = this.editors.get(containerId);
        if (editor) {
            const selection = editor.getSelection();
            if (selection) {
                editor.replaceSelection(text);
            } else {
                // No selection, insert at cursor
                const cursor = editor.getCursor();
                editor.replaceRange(text, cursor);
            }
        }
    }
};

// Watch for sidebar transitions and refresh editors
(function() {
    const refreshAllEditors = () => {
        if (window.monacoLoader) {
            // Single refresh - multiple timeouts were causing performance issues
            window.monacoLoader.refreshAll();
        }
    };
    
    const watchSidebars = () => {
        const sidebars = document.querySelectorAll('.sointu-song-manager, .git-dialog, .import-export-dialog, .settings-panel');
        sidebars.forEach(sidebar => {
            // Watch for class changes (when sidebar becomes visible)
            const sidebarObserver = new MutationObserver((mutations) => {
                mutations.forEach(mutation => {
                    if (mutation.type === 'attributes' && mutation.attributeName === 'class') {
                        const isVisible = sidebar.classList.contains('visible') || 
                                         window.getComputedStyle(sidebar).display !== 'none';
                        
                        if (isVisible) {
                            // Sidebar just became visible - aggressively refresh all editors
                            refreshAllEditors();
                        }
                    }
                });
            });
            
            sidebarObserver.observe(sidebar, {
                attributes: true,
                attributeFilter: ['class', 'style']
            });
            
            // Also listen for transition end
            sidebar.addEventListener('transitionend', refreshAllEditors, { once: false });
            
            // REMOVED: Separate display observer - class observer already handles this
        });
    };
    
    // Watch for Bootstrap tab changes (when switching between vertex/fragment/post tabs)
    const watchTabs = () => {
        // Listen for Bootstrap tab shown events
        document.addEventListener('shown.bs.tab', (e) => {
            // Refresh all editors when a tab is shown
            setTimeout(refreshAllEditors, 50);
        });
        
        // Also watch for tab-pane visibility changes (when tabs become visible)
        const tabPanes = document.querySelectorAll('.tab-pane');
        tabPanes.forEach(tabPane => {
            const tabObserver = new MutationObserver((mutations) => {
                mutations.forEach(mutation => {
                    if (mutation.type === 'attributes' && mutation.attributeName === 'class') {
                        // Check if tab became visible
                        if (tabPane.classList.contains('show') && tabPane.classList.contains('active')) {
                            // Find editor in this tab and refresh it
                            const editorContainer = tabPane.querySelector('.monaco-editor-container');
                            if (editorContainer) {
                                const containerId = editorContainer.id;
                                if (containerId && window.monacoLoader) {
                                    setTimeout(() => {
                                        window.monacoLoader.refresh(containerId);
                                    }, 100);
                                }
                            }
                        }
                    }
                });
            });
            
            tabObserver.observe(tabPane, {
                attributes: true,
                attributeFilter: ['class']
            });
        });
    };
    
    // REMOVED: Periodic check was causing high CPU usage
    // Editors will refresh via ResizeObserver and IntersectionObserver instead
    
    // Watch for sidebars and tabs once on load
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', () => {
            watchSidebars();
            watchTabs();
        });
    } else {
        watchSidebars();
        watchTabs();
    }
    
    // REMOVED: Document-level MutationObserver was causing high CPU usage
    // Sidebars are watched on initial load and when explicitly needed
})();

