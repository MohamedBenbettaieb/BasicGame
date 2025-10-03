// App.tsx
import { Routes, Route } from 'react-router-dom'
import { DevicesList } from './components/DevicesList'

function App() {
  return (
    <div className="container">
      <nav>...</nav>
      <Routes>
        <Route path="/" element={<DevicesList />} />
      </Routes>
    </div>
  )
}

export default App