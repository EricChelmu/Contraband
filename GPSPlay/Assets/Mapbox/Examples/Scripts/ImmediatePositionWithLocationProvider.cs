namespace Mapbox.Examples
{
	using Mapbox.Unity.Location;
    using Mapbox.Unity.Map;
    using TMPro;
    using UnityEngine;

	public class ImmediatePositionWithLocationProvider : MonoBehaviour
	{

		bool _isInitialized;

		LocationProviderFactory locationProviderFactory;
		AbstractMap map;

        ILocationProvider _locationProvider;
		ILocationProvider LocationProvider
		{
			get
			{
				if (_locationProvider == null)
				{
					_locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
				}

				return _locationProvider;
			}
		}

		Vector3 _targetPosition;

		void Start()
		{
			LocationProviderFactory.Instance.mapManager.OnInitialized += () => _isInitialized = true;
			locationProviderFactory = LocationProviderFactory.Instance;
			map = locationProviderFactory.mapManager;
		}

		void LateUpdate()
		{
			_isInitialized = true;
			if (/*_isInitialized*/map != null)
			{
				//var map = locationProviderFactory.mapManager;
				transform.localPosition = map.GeoToWorldPosition(LocationProvider.CurrentLocation.LatitudeLongitude);
			}
        }
	}
}