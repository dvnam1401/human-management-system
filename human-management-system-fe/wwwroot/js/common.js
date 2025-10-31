// Common utilities for HRMS Frontend

const HRMS = {
    // API Base URL
    apiBaseUrl: 'https://localhost:7126/api',

    // Show loading spinner
    showLoading: function (buttonElement) {
        if (buttonElement) {
            buttonElement.disabled = true;
            const originalText = buttonElement.innerHTML;
            buttonElement.setAttribute('data-original-text', originalText);
            buttonElement.innerHTML = '<span class="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>Đang xử lý...';
        }
    },

    // Hide loading spinner
    hideLoading: function (buttonElement) {
        if (buttonElement) {
            buttonElement.disabled = false;
            const originalText = buttonElement.getAttribute('data-original-text');
            if (originalText) {
                buttonElement.innerHTML = originalText;
            }
        }
    },

    // Show toast notification
    showToast: function (message, type = 'success') {
        // Remove existing toasts
        const existingToasts = document.querySelectorAll('.hrms-toast');
        existingToasts.forEach(toast => toast.remove());

        const toast = document.createElement('div');
        toast.className = `hrms-toast alert alert-${type} alert-dismissible fade show`;
        toast.setAttribute('role', 'alert');
        
        const icon = type === 'success' ? '✓' : (type === 'danger' ? '✕' : 'ℹ');
        
        toast.innerHTML = `
            <strong>${icon}</strong> ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        `;

        document.body.appendChild(toast);

        // Auto dismiss after 5 seconds
        setTimeout(() => {
            toast.classList.remove('show');
            setTimeout(() => toast.remove(), 150);
        }, 5000);
    },

    // Make AJAX call with credentials
    ajaxCall: async function (url, method = 'GET', data = null) {
        const options = {
            method: method,
            headers: {
                'Content-Type': 'application/json',
            },
            credentials: 'include' // Important for cookie authentication
        };

        if (data && method !== 'GET') {
            options.body = JSON.stringify(data);
        }

        try {
            const response = await fetch(url, options);
            const result = await response.json();

            if (!response.ok) {
                throw { status: response.status, data: result };
            }

            return { success: true, data: result };
        } catch (error) {
            console.error('AJAX Error:', error);
            
            if (error.status === 401) {
                window.location.href = '/Auth/Login';
                return { success: false, error: 'Unauthorized' };
            }

            return {
                success: false,
                error: error.data?.message || error.message || 'Đã xảy ra lỗi'
            };
        }
    },

    // Format date
    formatDate: function (dateString) {
        if (!dateString) return '';
        const date = new Date(dateString);
        return date.toLocaleDateString('vi-VN');
    },

    // Format datetime
    formatDateTime: function (dateString) {
        if (!dateString) return '';
        const date = new Date(dateString);
        return date.toLocaleString('vi-VN');
    },

    // Validate email
    validateEmail: function (email) {
        const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return re.test(email);
    },

    // Show validation error
    showValidationError: function (inputElement, message) {
        inputElement.classList.add('is-invalid');
        let errorDiv = inputElement.nextElementSibling;
        
        if (!errorDiv || !errorDiv.classList.contains('invalid-feedback')) {
            errorDiv = document.createElement('div');
            errorDiv.className = 'invalid-feedback';
            inputElement.parentNode.appendChild(errorDiv);
        }
        
        errorDiv.textContent = message;
    },

    // Clear validation error
    clearValidationError: function (inputElement) {
        inputElement.classList.remove('is-invalid');
        const errorDiv = inputElement.nextElementSibling;
        if (errorDiv && errorDiv.classList.contains('invalid-feedback')) {
            errorDiv.textContent = '';
        }
    },

    // Clear all validation errors in a form
    clearAllValidationErrors: function (formElement) {
        const inputs = formElement.querySelectorAll('.is-invalid');
        inputs.forEach(input => {
            this.clearValidationError(input);
        });
    }
};

// Add CSS for toast
const style = document.createElement('style');
style.textContent = `
    .hrms-toast {
        position: fixed;
        top: 20px;
        right: 20px;
        min-width: 300px;
        z-index: 9999;
        box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        animation: slideInRight 0.3s ease-out;
    }

    @keyframes slideInRight {
        from {
            transform: translateX(100%);
            opacity: 0;
        }
        to {
            transform: translateX(0);
            opacity: 1;
        }
    }

    .spinner-border-sm {
        width: 1rem;
        height: 1rem;
        border-width: 0.2em;
    }
`;
document.head.appendChild(style);

