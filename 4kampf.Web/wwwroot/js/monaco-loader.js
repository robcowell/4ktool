// Monaco Editor Loader for 4kampf
window.monacoLoader = {
    editors: new Map(),
    loaded: false,
    
    init: async function() {
        if (this.loaded) return Promise.resolve();
        
        return new Promise((resolve, reject) => {
            // Check if Monaco is already loaded
            if (typeof monaco !== 'undefined') {
                this.loaded = true;
                resolve();
                return;
            }
            
            // Load Monaco Editor from CDN
            const script = document.createElement('script');
            script.src = 'https://cdn.jsdelivr.net/npm/monaco-editor@0.45.0/min/vs/loader.js';
            document.head.appendChild(script);
            
            script.onload = () => {
                if (typeof require === 'undefined') {
                    reject(new Error('Monaco loader failed to load'));
                    return;
                }
                
                require.config({ paths: { vs: 'https://cdn.jsdelivr.net/npm/monaco-editor@0.45.0/min/vs' } });
                require(['vs/editor/editor.main'], () => {
                    this.loaded = true;
                    resolve();
                }, (err) => {
                    reject(err);
                });
            };
            script.onerror = () => reject(new Error('Failed to load Monaco Editor'));
        });
    },
    
    createEditor: function(containerId, language, initialValue, callbackRef) {
        if (!this.loaded) {
            console.error('Monaco Editor not loaded');
            return null;
        }
        
        const container = document.getElementById(containerId);
        if (!container) {
            console.error('Container not found:', containerId);
            return null;
        }
        
        const editor = monaco.editor.create(container, {
            value: initialValue || '',
            language: language || 'glsl',
            theme: 'vs-dark',
            automaticLayout: true,
            fontSize: 14,
            minimap: { enabled: false },
            scrollBeyondLastLine: false,
            wordWrap: 'on'
        });
        
        let debounceTimer;
        editor.onDidChangeModelContent(() => {
            clearTimeout(debounceTimer);
            debounceTimer = setTimeout(() => {
                if (callbackRef) {
                    callbackRef.invokeMethodAsync('OnChange', editor.getValue());
                }
            }, 500); // Debounce for 500ms
        });
        
        this.editors.set(containerId, editor);
        return editor;
    },
    
    setValue: function(containerId, value) {
        const editor = this.editors.get(containerId);
        if (editor) {
            editor.setValue(value);
        }
    },
    
    getValue: function(containerId) {
        const editor = this.editors.get(containerId);
        return editor ? editor.getValue() : '';
    },
    
    destroyEditor: function(containerId) {
        this.dispose(containerId);
    },
    
    undo: function(containerId) {
        const editor = this.editors.get(containerId);
        if (editor) {
            editor.trigger('source', 'undo', null);
        }
    },
    
    redo: function(containerId) {
        const editor = this.editors.get(containerId);
        if (editor) {
            editor.trigger('source', 'redo', null);
        }
    },
    
    canUndo: function(containerId) {
        const editor = this.editors.get(containerId);
        return editor ? editor.getModel()?.canUndo() || false : false;
    },
    
    canRedo: function(containerId) {
        const editor = this.editors.get(containerId);
        return editor ? editor.getModel()?.canRedo() || false : false;
    },
    
    showFind: function(containerId) {
        const editor = this.editors.get(containerId);
        if (editor) {
            editor.getAction('actions.find').run();
        }
    },
    
    setLineNumbers: function(containerId, visible) {
        const editor = this.editors.get(containerId);
        if (editor) {
            editor.updateOptions({ lineNumbers: visible ? 'on' : 'off' });
        }
    },
    
    gotoLine: function(containerId, lineNumber) {
        const editor = this.editors.get(containerId);
        if (editor) {
            editor.revealLineInCenter(lineNumber);
            editor.setPosition({ lineNumber: lineNumber, column: 1 });
            editor.focus();
        }
    },
    
    insertText: function(containerId, text) {
        const editor = this.editors.get(containerId);
        if (editor) {
            const selection = editor.getSelection();
            editor.executeEdits('', [{
                range: selection,
                text: text
            }]);
        }
    },
    
    replaceSelection: function(containerId, text) {
        const editor = this.editors.get(containerId);
        if (editor) {
            const selection = editor.getSelection();
            editor.executeEdits('', [{
                range: selection,
                text: text
            }]);
        }
    },
    
    dispose: function(containerId) {
        const editor = this.editors.get(containerId);
        if (editor) {
            editor.dispose();
            this.editors.delete(containerId);
        }
    }
};

