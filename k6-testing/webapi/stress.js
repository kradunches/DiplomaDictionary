// javascript
import http from 'k6/http';
import {check, sleep} from 'k6';
import {htmlReport} from 'https://raw.githubusercontent.com/benc-uk/k6-reporter/main/dist/bundle.js';

// стресс-тест
export const options = {
    scenarios: {
        stress_test: {
            executor: 'ramping-vus',
            startVUs: 0,
            stages: [
                {duration: '2m', target: 10},   // начальная лёгкая нагрузка
                {duration: '2m', target: 30},   // рост
                {duration: '3m', target: 50},   // умеренная нагрузка
                {duration: '3m', target: 80},   // выше средней
                {duration: '4m', target: 120},  // интенсивная
                {duration: '4m', target: 160},  // высокая
                {duration: '4m', target: 200},  // пиковая (стресс)
                {duration: '3m', target: 0},    // сброс нагрузки
            ],
            gracefulRampDown: '1m',
        },
    },
    thresholds: {
        http_req_failed: ['rate<0.05'], // допускаем до 5% ошибок под стрессом
        http_req_duration: [
            'p(95)<1500',
            'p(99)<3000',
        ],
    },
};

export default function () {
    const res = http.get('http://localhost:5093/controller/GetAllTerms');

    check(res, {
        'status is 200': (r) => r.status === 200,
    });

    sleep(1);
}

export function handleSummary(data) {
    return {
        'results/stress_getallterms_report.html': htmlReport(data),
    };
}
