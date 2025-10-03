import React from 'react';
import { useDevices } from '../hooks/useDevices';

export const DevicesList: React.FC = () => {
  const { devices, loading, error, refetch } = useDevices();

  if (loading) {
    return (
      <div className="text-center py-5">
        <div className="spinner-border text-primary" role="status" aria-hidden="true"></div>
        <div className="mt-2">Loading devices…</div>
      </div>
    );
  }

  if (error) {
    return <div className="alert alert-danger">Error: {error}</div>;
  }

  if (devices.length === 0) {
    return (
      <div className="text-center py-4">
        <h3 className="mb-3">No devices yet</h3>
        <button className="btn btn-primary" onClick={refetch}>Refresh</button>
      </div>
    );
  }

  return (
    <div>
      <div className="d-flex justify-content-between align-items-center mb-3">
        <h2 className="h5 m-0">Devices</h2>
        <button className="btn btn-outline-secondary btn-sm" onClick={refetch}>
          Refresh
        </button>
      </div>

      <div className="table-responsive">
        <table className="table table-striped table-hover align-middle">
          <thead className="table-light">
            <tr>
              <th>Name</th>
              <th>Type</th>
              <th>Available</th>
              <th>Current Session</th>
            </tr>
          </thead>
          <tbody>
            {devices.map(d => (
              <tr key={d.id}>
                <td>{d.name}</td>
                <td>{d.type}</td>
                <td>
                  {d.isAvailable ? (
                    <span className="badge bg-success">Yes</span>
                  ) : (
                    <span className="badge bg-danger">No</span>
                  )}
                </td>
                <td>{d.currentSessionId ?? '-'}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};