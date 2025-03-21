const apiBaseUrl = 'http://localhost:5132/api/school'; 

document.getElementById('addStudentForm').addEventListener('submit', async (e) => {
    e.preventDefault();

    const student = {
        gender: document.getElementById('gender').value,
        dateOfBirth: new Date(document.getElementById('dateOfBirth').value),
        class: document.getElementById('className').value
    };

    try {
        const response = await fetch(`${apiBaseUrl}/students`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(student)
        });
        const result = await response.json();
        document.getElementById('addResult').innerHTML = result.message;
    } catch (error) {
        document.getElementById('addResult').innerHTML = 'Error adding student: ' + error.message;
    }
});

async function getAllStudents() {
    try {
        const response = await fetch(`${apiBaseUrl}/students`);
        const students = await response.json();
        displayStudents(students, 'allStudentsResult');
    } catch (error) {
        document.getElementById('allStudentsResult').innerHTML = 'Error: ' + error.message;
    }
}

async function getStudentsByClass() {
    const className = document.getElementById('classFilter').value;
    if (!className) {
        alert('Please enter a class name');
        return;
    }

    try {
        const response = await fetch(`${apiBaseUrl}/students/class/${className}`);
        const students = await response.json();
        displayStudents(students, 'classStudentsResult');
    } catch (error) {
        document.getElementById('classStudentsResult').innerHTML = 'Error: ' + error.message;
    }
}

async function checkClassroomFit() {
    const className = document.getElementById('fitClassName').value;
    const roomName = document.getElementById('roomName').value;

    if (!className || !roomName) {
        alert('Please enter both class name and room name');
        return;
    }

    try {
        const response = await fetch(`${apiBaseUrl}/classroom/fit?className=${className}&roomName=${roomName}`);
        const result = await response.json();
        document.getElementById('fitResult').innerHTML =
            `Class ${result.className} ${result.canFit ? 'can' : 'cannot'} fit in room ${result.roomName}`;
    } catch (error) {
        document.getElementById('fitResult').innerHTML = 'Error: ' + error.message;
    }
}

function displayStudents(students, elementId) {
    const container = document.getElementById(elementId);
    if (students.length === 0) {
        container.innerHTML = 'No students found';
        return;
    }

    const html = students.map(student => `
        <div>
            Gender: ${student.genderPerson}<br>
            Date of Birth: ${new Date(student.dateOfBirth).toLocaleDateString()}<br>
            Class: ${student.class}<br>
            <hr>
        </div>
    `).join('');
    container.innerHTML = html;
}