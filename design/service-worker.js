self.addEventListener('push', function(event) {
	event.waitUntil(
	
		self.registration.showNotification('Critical Alert', {
			body: 'Alert Message',
		})
	);
});